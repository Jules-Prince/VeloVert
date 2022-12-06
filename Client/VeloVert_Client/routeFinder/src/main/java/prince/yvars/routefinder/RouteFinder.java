package prince.yvars.routefinder;

import org.apache.activemq.ActiveMQConnectionFactory;
import org.json.JSONException;
import org.json.JSONObject;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageConsumer;
import javax.naming.InitialContext;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class RouteFinder implements javax.jms.MessageListener {
    private static final String DEFAULT_BROKER_NAME = "tcp://localhost:61616";

    public static final String ANSI_RESET = "\u001B[0m";
    public static final String ANSI_GREEN = "\u001B[32m";
    public static final String ANSI_RED = "\u001B[31m";

    private javax.jms.Connection connect = null;
    private javax.jms.Session sendSession = null;
    private javax.jms.Session receiveSession = null;
    private javax.jms.MessageProducer sender = null;
    private static InitialContext context = null;

    public static List<GpsPoint> listGpsPoint = new ArrayList<>();

    public void factory(String username, String password, String broker) throws JMSException {
        javax.jms.ConnectionFactory factory;
        factory = new ActiveMQConnectionFactory(username, password, broker);
        connect = factory.createConnection(username, password);
    }

    public javax.jms.Queue queueBuild(String myQueueName) throws JMSException {

        sendSession = connect.createSession(false, javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.Queue queue = sendSession.createQueue(myQueueName);
        return queue;
    }

    public javax.jms.MessageConsumer conommateur(javax.jms.Queue queue) throws JMSException {
        receiveSession = connect.createSession(false, javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
        return qReceiver;
    }

    public void start(javax.jms.MessageConsumer qReceiver) throws JMSException {
        qReceiver.setMessageListener(this);
        connect.start();
    }

    public static String getItineary(String depart, String arrivee) {
        /*INavigationveloserviceSOAP n = null;
        try {
            NavigationveloserviceSOAP navigationveloserviceSOAP = new NavigationveloserviceSOAP();
            n = navigationveloserviceSOAP.getBasicHttpBindingINavigationveloserviceSOAP();
        } catch (Exception e) {
            System.out.println(ANSI_RED + "ERROR : Unable to connect to the server" + ANSI_RESET);
            System.exit(0);
        }

        String myQueue = n.getCheminAVelo(depart, arrivee);

        if (!myQueue.equals("Unknown city") && !myQueue.equals("No station")) {
            RouteFinder client = new RouteFinder();
            //1
            try {
                client.factory("user", "user", DEFAULT_BROKER_NAME);

                //2
                javax.jms.Queue queue = client.queueBuild(myQueue);
                //3
                MessageConsumer qReceiver = client.conommateur(queue);
                //4
                client.start(qReceiver);
            } catch (JMSException e) {
                throw new RuntimeException(e);
            }
        }

        JSONObject json = new JSONObject();
        try {
            json.put("gpsPoints", listGpsPoint);
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        return json.toString();*/
        return null;
    }

    public static void main(String[] args) throws JMSException {
        Scanner scanner = new Scanner(System.in);

        System.setProperty("javax.xml.soap.MetaFactory",
                "com.sun.xml.messaging.saaj.soap.SAAJMetaFactoryImpl");

        System.out.println("\n" + ANSI_GREEN +
                "    //   ) )                                                  //   ) )\n" +
                "   //            __        ___        ___         __         //___/ /     ( )     / ___      ___\n" +
                "  //  ____     //  ) )   //___) )   //___) )   //   ) )     / __  (      / /     //\\ \\     //___) )\n" +
                " //    / /    //        //         //         //   / /     //    ) )    / /     //  \\ \\   //\n" +
                "((____/ /    //        ((____     ((____     //   / /     //____/ /    / /     //    \\ \\ ((____" + ANSI_RESET);

        System.out.println();
        System.out.println();

        System.out.println("Hello dear customer, welcome to the Green Bike application. \n" +
                "Enter the destination of departure and arrival to have your journey by bike. \nThis application will allow you to get to the nearest station. ");
        System.out.println();


        /*INavigationveloserviceSOAP n = null;
        try{
            NavigationveloserviceSOAP navigationveloserviceSOAP = new NavigationveloserviceSOAP();
            n = navigationveloserviceSOAP.getBasicHttpBindingINavigationveloserviceSOAP();
        }catch (Exception e){
            e.printStackTrace();
            System.out.println(ANSI_RED + "ERROR : Unable to connect to the server" + ANSI_RESET);
            System.exit(0);
        }

        while(true) {
            System.out.print("D'où partez vous ? : ");
            String depart = scanner.nextLine();
            System.out.print("Où allez vous ? : ");
            String arrivee = scanner.nextLine();
            System.out.println();


            //String myQueue = n.getCheminAVelo("33 Rue Edouard Nieuport, 69008 Lyon", "12 Bd Fernand Bonnefoy, 13010 Marseille");
            String myQueue = n.getCheminAVelo(depart, arrivee);

            System.out.println("myIdQueue : " + myQueue);
            System.out.println();

            if (!myQueue.equals("Unknow city") && !myQueue.equals("No station")) {
                RouteFinder client = new RouteFinder();
                //1
                client.factory("user", "user", DEFAULT_BROKER_NAME);
                //2
                javax.jms.Queue queue = client.queueBuild(myQueue);
                //3
                MessageConsumer qReceiver = client.conommateur(queue);
                //4
                client.start(qReceiver);
            }
        }*/
    }



    @Override
    public void onMessage(Message aMessage) {
        try {
            if (aMessage instanceof javax.jms.TextMessage) {
                javax.jms.TextMessage textMessage = (javax.jms.TextMessage) aMessage;

                // This handler reads a single String from the
                // message and prints it to the standard output.
                try {
                    String string = textMessage.getText();
                    System.out.println(string);


                    JSONObject obj = null;
                    try {
                        obj = new JSONObject(string);
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                    try {
                        String latitude = obj.getString("latitude");
                        String longitude = obj.getString("longitude");
                        String transportType = obj.getString("tansportType");
                        listGpsPoint.add(new GpsPoint(latitude, longitude, TransportType.valueOf(transportType)));
                        System.out.println("Latitude : " + latitude);
                        System.out.println("Longitude : " + longitude);
                        System.out.println("Transport Type : " + transportType);
                        System.out.println();
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                } catch (javax.jms.JMSException jmse) {
                    jmse.printStackTrace();
                }
            } else {
                System.out.println("Warning: A message was discarded because it could not be processed " +
                        "as a javax.jms.TextMessage.");
            }

        } catch (java.lang.RuntimeException rte) {
            rte.printStackTrace();
        }
    }
}
