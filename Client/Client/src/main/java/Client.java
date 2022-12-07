import com.soap.ws.client.generated.INavigationveloserviceSOAP;
import com.soap.ws.client.generated.NavigationveloserviceSOAP;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.json.JSONException;
import org.json.JSONObject;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageConsumer;
import javax.naming.InitialContext;
import java.util.Scanner;

public class Client implements javax.jms.MessageListener{
    private static final String DEFAULT_BROKER_NAME = "tcp://localhost:61616";

    // Color for fun
    public static final String ANSI_RESET = "\u001B[0m";
    public static final String ANSI_GREEN = "\u001B[32m";
    public static final String ANSI_RED = "\u001B[31m";

    private static javax.jms.Connection connect = null;
    private static javax.jms.Session sendSession = null;
    private static javax.jms.Session receiveSession = null;
    private static javax.jms.MessageProducer sender = null;
    private static InitialContext context = null;

    // ================================================
    //             ActiveMQ Consumer
    // ================================================

    /**
     * Create conection
     * @param username
     * @param password
     * @param broker
     * @throws JMSException
     */
    public void factory(String username, String password, String broker) throws JMSException {
        javax.jms.ConnectionFactory factory;
        factory = new ActiveMQConnectionFactory(username, password, broker);
        try {
            connect = factory.createConnection (username, password);
        }catch (Exception e) {
            System.out.println();
            System.out.println(ANSI_RED + "Error -------- : ActiveMQ is not working well." + ANSI_RESET);
            connect = null;
        }
    }

    /**
     * Connect to the queue with the id
     * @param myQueueName
     * @return
     * @throws JMSException
     */
    public javax.jms.Queue queueBuild(String myQueueName) throws JMSException {

        sendSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.Queue queue = sendSession.createQueue (myQueueName);
        return  queue;
    }

    /**
     * Created a consumer
     * @param queue
     * @return
     * @throws JMSException
     */
    public javax.jms.MessageConsumer conommateur(javax.jms.Queue queue) throws JMSException {
        receiveSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
        return qReceiver;
    }

    /**
     * Start the connection
     * @param qReceiver
     * @throws JMSException
     */
    public void start(javax.jms.MessageConsumer qReceiver) throws JMSException {
        qReceiver.setMessageListener(this);
        connect.start();
    }

    // ================================================
    //                APPICATION
    // ================================================

    /**
     * main
     * @param args
     * @throws JMSException
     */
    public static void main(String[] args) throws JMSException {
        Scanner scanner = new Scanner(System.in);
        boolean routingServeIsOn = false;

        title();

        // ========================================
        // [ 1 ] Attempt to connect to our routing server
        // ========================================
        INavigationveloserviceSOAP n = null;

        while(!routingServeIsOn) {
            try {
                NavigationveloserviceSOAP navigationveloserviceSOAP = new NavigationveloserviceSOAP();
                n = navigationveloserviceSOAP.getBasicHttpBindingINavigationveloserviceSOAP();
                routingServeIsOn = true;
                System.out.println("[ "+ANSI_GREEN+ "Connected"+ANSI_RESET+" ] to the routing server.");
            } catch (Exception e) {
                System.out.println(ANSI_RED + "ERROR : Unable to connect to the server" + ANSI_RESET);
            }

            try {
                Thread.sleep(5000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        // ========================================
        // [ 2 ] Loop to ask for itinerary
        // ========================================
        while(true) {

            bar();

            boolean validAddress = false;
            String depart = "";
            String arrivee = "";

            // ========================================
            // [ 3 ] Recovering addresses
            // ========================================

            while(!validAddress) {
                System.out.print("D'où partez vous ? : ");
                depart = scanner.nextLine();
                System.out.print("Où allez vous ? : ");
                arrivee = scanner.nextLine();
                System.out.println();

                if(isFalseEntries(depart,arrivee)){
                    System.out.println();
                }else{
                    validAddress = true;
                }
            }

            // ========================================
            // [ 4 ] Request to our routing server
            // ========================================

            // Recovery of the connection id to AvctiveMQ
            String myQueue = "";
            try {
                myQueue = n.getCheminAVelo(depart, arrivee);
            }catch (Exception e){
                System.out.println("TEST");
            }


            System.out.println("myIdQueue : [ "+ ANSI_GREEN + myQueue + ANSI_RESET +" ]");
            System.out.println();

            // ========================================
            // [ 5 ] Connaction to the ActiveMQ queue
            // ========================================

            Client client = new Client();
            //1
            client.factory("user", "user", DEFAULT_BROKER_NAME);
            if(connect != null) { // ActiveMQ must be running
                //2
                javax.jms.Queue queue = client.queueBuild(myQueue);
                //3
                MessageConsumer qReceiver = client.conommateur(queue);
                //4
                client.start(qReceiver);
            }
            // ========================================
            // [ 6 ] 5 seconds wait with the next request
            // ========================================
            try {
                Thread.sleep(5000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    /**
     * This method is called when a message is received from the queue.
     * @param aMessage
     */
    @Override
    public void onMessage(Message aMessage) {
        try
        {
            if (aMessage instanceof javax.jms.TextMessage)
            {
                javax.jms.TextMessage textMessage = (javax.jms.TextMessage) aMessage;

                // This handler reads a single String from the
                // message and prints it to the standard output.
                try
                {
                    // ===========================================
                    // [ 1 ] JSON message retrieval
                    // ===========================================

                    String string = textMessage.getText();
                    JSONObject obj = null;
                    try {
                        obj = new JSONObject(string);
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }

                    boolean noErrorEncountered = false;

                    // ===========================================
                    // [ 2 ] If we have an error message, then we display it.
                    // ===========================================

                    try {
                        String error = obj.getString("error");
                        System.out.println(ANSI_RED + "Error -------- : " + error + ANSI_RESET);
                    } catch (JSONException e) {
                        noErrorEncountered = true;
                    }
                    // ===========================================
                    // [ 3 ] If there is no error message, then the information of the received step is displayed.
                    // ===========================================

                    if(noErrorEncountered) {

                        String latitude = null;

                        try {
                            latitude = obj.getString("latitude");
                            String longitude = obj.getString("longitude");
                            String transportType = obj.getString("tansportType");

                            System.out.println("Latitude ----- : [ " + ANSI_GREEN + latitude + ANSI_RESET + " ]");
                            System.out.println("Longitude ---- : [ " + ANSI_GREEN + longitude + ANSI_RESET + " ]");
                            System.out.println("Transport Type : [ " + ANSI_GREEN + transportType + ANSI_RESET + " ]");
                            System.out.println();
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }

                }
                catch (javax.jms.JMSException jmse)
                {
                    jmse.printStackTrace();
                }
            }
            else
            {
                System.out.println ("Warning: A message was discarded because it could not be processed " +
                        "as a javax.jms.TextMessage.");
            }

        }
        catch (java.lang.RuntimeException rte)
        {
            rte.printStackTrace();
        }
    }

    /**
     * Checks if the strings are good
     * @param depart
     * @param arrivee
     * @return
     */
    private static boolean isFalseEntries(String depart, String arrivee){
        if(depart.length() == 0 || arrivee.length() == 0){
            System.out.println();
            System.out.println(ANSI_RED + "Error -------- : empty address. Please try again." + ANSI_RESET);
            return true;
        }
        if(depart.charAt(0) == '.' || depart.charAt(0) == ';'){
            System.out.println();
            System.out.println(ANSI_RED + "Error -------- : Character ';' and '.' forbidden first." + ANSI_RESET);
            return true;
        }
        return false;
    }

    // ===========================================
    //                FUN
    // ===========================================
    // https://patorjk.com/software/taag/#p=display&f=Electronic&t=Velo-Vert%0A---------

    /**
     * Title
     */
    private static void title(){
        System.out.println();
        System.out.println();
        System.out.println(ANSI_GREEN + " ▄               ▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄            ▄▄▄▄▄▄▄▄▄▄▄       ▄               ▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄ \n" +
                "▐░▌             ▐░▌▐░░░░░░░░░░░▌▐░▌          ▐░░░░░░░░░░░▌     ▐░▌             ▐░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌\n" +
                " ▐░▌           ▐░▌ ▐░█▀▀▀▀▀▀▀▀▀ ▐░▌          ▐░█▀▀▀▀▀▀▀█░▌      ▐░▌           ▐░▌ ▐░█▀▀▀▀▀▀▀▀▀ ▐░█▀▀▀▀▀▀▀█░▌ ▀▀▀▀█░█▀▀▀▀ \n" +
                "  ▐░▌         ▐░▌  ▐░▌          ▐░▌          ▐░▌       ▐░▌       ▐░▌         ▐░▌  ▐░▌          ▐░▌       ▐░▌     ▐░▌     \n" +
                "   ▐░▌       ▐░▌   ▐░█▄▄▄▄▄▄▄▄▄ ▐░▌          ▐░▌       ▐░▌        ▐░▌       ▐░▌   ▐░█▄▄▄▄▄▄▄▄▄ ▐░█▄▄▄▄▄▄▄█░▌     ▐░▌     \n" +
                "    ▐░▌     ▐░▌    ▐░░░░░░░░░░░▌▐░▌          ▐░▌       ▐░▌         ▐░▌     ▐░▌    ▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌     ▐░▌     \n" +
                "     ▐░▌   ▐░▌     ▐░█▀▀▀▀▀▀▀▀▀ ▐░▌          ▐░▌       ▐░▌          ▐░▌   ▐░▌     ▐░█▀▀▀▀▀▀▀▀▀ ▐░█▀▀▀▀█░█▀▀      ▐░▌     \n" +
                "      ▐░▌ ▐░▌      ▐░▌          ▐░▌          ▐░▌       ▐░▌           ▐░▌ ▐░▌      ▐░▌          ▐░▌     ▐░▌       ▐░▌     \n" +
                "       ▐░▐░▌       ▐░█▄▄▄▄▄▄▄▄▄ ▐░█▄▄▄▄▄▄▄▄▄ ▐░█▄▄▄▄▄▄▄█░▌            ▐░▐░▌       ▐░█▄▄▄▄▄▄▄▄▄ ▐░▌      ▐░▌      ▐░▌     \n" +
                "        ▐░▌        ▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌             ▐░▌        ▐░░░░░░░░░░░▌▐░▌       ▐░▌     ▐░▌     \n" +
                "         ▀          ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀               ▀          ▀▀▀▀▀▀▀▀▀▀▀  ▀         ▀       ▀      " + ANSI_RESET);
        System.out.println();
        System.out.println();
        System.out.println("Hello dear customer, welcome to the Green Bike application. \n" +
                "Enter the destination of departure and arrival to have your journey by bike. \nThis application will allow you to get to the nearest station. ");
        System.out.println();
        System.out.println("Example of departure : ["+ANSI_GREEN+" 33 Rue Edouard Nieuport, 69008 Lyon "+ANSI_RESET+"]");
        System.out.println("Example of arrival : ["+ANSI_GREEN+" 12 Bd Fernand Bonnefoy, 13010 Marseille "+ANSI_RESET+"]");
        System.out.println();
    }

    /**
     * Bar
     */
    private static void bar(){
        System.out.println();
        System.out.println(ANSI_GREEN + " ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄▄▄▄▄         \n" +
                "▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌▐░░░░░░░░░░░▌        \n" +
                " ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  ▀▀▀▀▀▀▀▀▀▀▀  " + ANSI_RESET);
        System.out.println();
    }
}
