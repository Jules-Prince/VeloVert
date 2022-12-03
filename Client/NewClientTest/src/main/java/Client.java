import com.soap.ws.client.generated.INavigationveloserviceSOAP;
import com.soap.ws.client.generated.NavigationveloserviceSOAP;
import org.apache.activemq.ActiveMQConnectionFactory;

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
        // 1 Création de la connexion pour connecter correctement le code (la JVM) au broker.
        // ( la documentation si nécessaire est ici http://docs.oracle.com/javaee/7/api/javax/jms/Connection.html
        // pour plus d'informations sur ce qu'est une connexion)
        javax.jms.ConnectionFactory factory;
        factory = new ActiveMQConnectionFactory(username, password, broker);
        connect = factory.createConnection (username, password);
    }

    public javax.jms.Queue queueBuild(String myQueueName) throws JMSException {
        // 2 Code du producteur qui crée une session, http://docs.oracle.com/javaee/7/api/javax/jms/Session.html
        // qui crée explicitement un point d'accès à la file d'attente en utilisant son nom (notez que c'est non
        // compatible JMS mais spécifique à ActiveMQ ) et se fait producteur .
        // Ensuite, le code devra produire des messages, mais attendez la prochaine
        // question pour programmer ces productions de messages !
        sendSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.Queue queue = sendSession.createQueue (myQueueName);
        return  queue;
    }

    public javax.jms.MessageConsumer conommateur(javax.jms.Queue queue) throws JMSException {
        // 3 Code du consommateur qui crée une session et se transforme en consommateur à partir de
        // la file d'attente (la file d'attente qui a été explicitement identifiée à l'étape 2
        // ci-dessus, dans le code). La partie consommateur du code encode la méthode onMessage ( ) (et en tant que
        // telle, déclare qu'elle implémente l' interface MessageListener JMS). Dans cette question, l'auditeur invoque
        // simplement une impression sur la sortie standard pour dire qu'un message a été reçu (sans essayer encore
        // de consulter le contenu du message)
        receiveSession = connect.createSession(false,javax.jms.Session.AUTO_ACKNOWLEDGE);
        javax.jms.MessageConsumer qReceiver = receiveSession.createConsumer(queue);
        return qReceiver;
    }

    public void start(javax.jms.MessageConsumer qReceiver) throws JMSException {
        // 4 Création de messages et leur envoi dans la file d'attente, à l'aide de l'application
        // d'administration Web
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
    public void onMessage(Message message) {
        System.out.println("Le message : " + message);
    }
}
