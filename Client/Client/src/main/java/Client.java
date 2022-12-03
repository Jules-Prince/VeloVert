import com.soap.ws.client.generated.INavigationveloserviceSOAP;
import com.soap.ws.client.generated.NavigationveloserviceSOAP;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageConsumer;
import javax.naming.InitialContext;

public class Client implements javax.jms.MessageListener{
    private static final String DEFAULT_BROKER_NAME = "tcp://localhost:61616";
    private static final String DEFAULT_PASSWORD = "password";
    private static final int    MESSAGE_LIFESPAN = 1800000;  // milliseconds (30 minutes)

    private javax.jms.Connection connect = null;
    private javax.jms.Session sendSession = null;
    private javax.jms.Session receiveSession = null;
    private javax.jms.MessageProducer sender = null;
    private static InitialContext context = null;

    public void factory(String username, String password, String broker) throws JMSException {
        javax.jms.ConnectionFactory factory;
        factory = new ActiveMQConnectionFactory(username, password, broker);
        connect = factory.createConnection (username, password);
    }

    public javax.jms.Queue queueBuild(String myQueueName) throws JMSException {

        sendSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.Queue queue = sendSession.createQueue (myQueueName);
        return  queue;
    }

    public javax.jms.MessageConsumer conommateur(javax.jms.Queue queue) throws JMSException {
        receiveSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
        return qReceiver;
    }

    public void start(javax.jms.MessageConsumer qReceiver) throws JMSException {
        qReceiver.setMessageListener(this);
        connect.start();
    }

    public static void main(String[] args) throws JMSException {
        System.out.println("Hello World! we are going to test a SOAP client written in Java");
        NavigationveloserviceSOAP navigationveloserviceSOAP = new NavigationveloserviceSOAP();
        INavigationveloserviceSOAP n = navigationveloserviceSOAP.getBasicHttpBindingINavigationveloserviceSOAP();
        String myQueue = n.getCheminAVelo("33 Rue Edouard Nieuport, 69008 Lyon", "12 Bd Fernand Bonnefoy, 13010 Marseille");
        System.out.println("myQueue : " + myQueue);


        Client client = new Client();
        //1
        client.factory("user", "user", DEFAULT_BROKER_NAME);
        //2
        javax.jms.Queue queue = client.queueBuild(myQueue);
        //3
        MessageConsumer qReceiver = client.conommateur(queue);
        //4
        client.start(qReceiver);
    }

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
                    String string = textMessage.getText();
                    System.out.println( string );


                    JSONObject obj = null;
                    try {
                        obj = new JSONObject(string);
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                    try {
                        String latitude = obj.getString("latitude");
                        String longitude = obj.getString("longitude");

                        System.out.println("Latitude : " + latitude);
                        System.out.println("Longitude : " + longitude);
                        System.out.println();
                    } catch (JSONException e) {
                        e.printStackTrace();
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
}
