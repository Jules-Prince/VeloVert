using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;

namespace RoutingServer
{
    public class ActiveMQ
    {
        /**
         * This class is responsible for producing a message in activeMQ
         */

        /**
         * This method puts in activeMQ the list of stops (longitude + latitude), 
         * as well as the type of transport (bicycle or on foot).
         * 
         * This method creates a tail name with a GUID as name. 
         * It returns GUID, and this will be the return element to the client who made the request.
         * He will then be able to connect to the queue to collect the data related to his route choice. 
         */
        public Guid producer(Positions positions)
        {
            Guid guid = Guid.NewGuid(); // Produces a GUID

            // Create a Connection Factory.

            Uri connecturi = new Uri("activemq:tcp://localhost:61616");

            ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);

            // Create a single Connection from the Connection Factory.
            IConnection connection = null;
            try
            {
                connection = connectionFactory.CreateConnection();
                connection.Start();
            }catch(Exception ex)
            {
                Console.WriteLine("Error : Failed to connect to ActiveMQ");
                return guid;
            }

            // Create a session from the Connection.
            Apache.NMS.ISession session = connection.CreateSession();

            // Use the session to target a queue.
            IDestination destination = session.GetQueue(guid.ToString());

            // Create a Producer targetting the selected queue.
            IMessageProducer producer = session.CreateProducer(destination);

            // You may configure everything to your needs, for instance:
            producer.DeliveryMode = MsgDeliveryMode.NonPersistent;



            // Finally, to send messages:
            foreach (Position position in positions.step)
            {
                string latitude = position.latitude.ToString();
                string longitude = position.longitude.ToString();
                string transportType = position.type.ToString();

                // Builds the JSON
                string jsonData = @"{'latitude':'" + latitude + "','longitude':'" + longitude + "','tansportType':'" + transportType + "'}";
                ITextMessage message = session.CreateTextMessage(jsonData);

                producer.Send(message);
            }


            Console.WriteLine("Message sent, check ActiveMQ web interface to confirm.");

            // Close your session, connection and producer when finished.
            producer.Close();
            session.Close();
            connection.Close();

            return guid;
        }

        /**
         * This method puts the error message in the queue for the client who made the request. 
         * This method returns a guid string so that the client can connect to it. 
         */
        public Guid errorProducer(string errorMesage)
        {
            Guid guid = Guid.NewGuid();

            // Create a Connection Factory.
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);
            // Create a single Connection from the Connection Factory.
            IConnection connection = connectionFactory.CreateConnection();
            connection.Start();

            // Create a session from the Connection.
            Apache.NMS.ISession session = connection.CreateSession();

            // Use the session to target a queue.
            IDestination destination = session.GetQueue(guid.ToString());

            // Create a Producer targetting the selected queue.
            IMessageProducer producer = session.CreateProducer(destination);

            // You may configure everything to your needs, for instance:
            producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

            // Error message
            string jsonData = @"{'error':'" + errorMesage + "'}";
            ITextMessage message = session.CreateTextMessage(jsonData);
            producer.Send(message);
            
            // Close your session, connection and producer when finished.
            producer.Close();
            session.Close();
            connection.Close();

            return guid;
        }
    }
}
