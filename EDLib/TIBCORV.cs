using System;
using TIBCO.Rendezvous;

namespace EDLib.TIBCORV
{
    /// <summary>
    /// TIBCO Rendezvous services
    /// </summary>
    internal class NamespaceDoc { }

    /// <summary>
    /// Function pointer of listener callback function
    /// </summary>
    /// <param name="listener">Caller of callback function</param>
    /// <param name="messageReceivedEventArgs">Message received by callback function</param>
    public delegate void ListenerFunc(object listener , MessageReceivedEventArgs messageReceivedEventArgs);

    /// <summary>
    /// Listen to TIBCO Rendezvous services. This class can let you specify multiple net transports and listener callbacks.
    /// </summary>
    public class TIBCORVListener
    {
        private int N = 0;
        private Transport[] transport;

        /// <summary>
        /// Initiate listener with parameters
        /// </summary>
        /// <param name="service">String array of service parameters</param>
        /// <param name="network">String array of network parameters</param>
        /// <param name="daemon">String array of daemon parameters</param>
        /// <exception cref="ArgumentException">Parameter arrays must have same length</exception>
        public TIBCORVListener(string[] service , string[] network , string[] daemon) {
            N = service.Length;
            if (N != network.Length || N != daemon.Length)
                throw new ArgumentException("Parameter arrays must have same length");

            transport = new Transport[N];
            try {
                /* Create internal TIB/Rendezvous machinery */
                if (TIBCO.Rendezvous.Environment.IsIPM()) {
                    TIBCO.Rendezvous.Environment.Open(".\\tibrvipm.cfg");
                } else {
                    TIBCO.Rendezvous.Environment.Open();
                }
            } catch (RendezvousException exception) {
                Console.Error.WriteLine("Failed to open Rendezvous Environment: {0}" , exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
                Console.ReadKey();
                System.Environment.Exit(1);
            }
            Console.WriteLine("Opened Environment"); //
            for (int i = 0; i < N; i++) {
                // Create Network transport
                try {
                    transport[i] = new NetTransport(service[i] , network[i] , daemon[i]);
                    Console.WriteLine("Created Net Transport"); //
                } catch (RendezvousException exception) {
                    Console.Error.WriteLine("Failed to create NetTransport");
                    Console.Error.WriteLine(exception.StackTrace);
                    Console.ReadKey();
                    System.Environment.Exit(1);
                }
            }
        }

        /// <summary>
        /// Start listen to topics with callback functions.
        /// </summary>
        /// <param name="topic">String array of topic parameters</param>
        /// <param name="callBack">Callback function to be called on reveiviing message.</param>
        /// <exception cref="ArgumentException">Parameter arrays must have length</exception>
        public void Listen(string[] topic , ListenerFunc[] callBack) {
            if (N != topic.Length || N != callBack.Length)
                throw new ArgumentException("Parameter arrays must have length " + N);

            Listener[] listeners = new Listener[N];
            for (int i = 0; i < N; i++) {
                // Create listeners for specified subjects                
                try {
                    listeners[i] = new Listener(Queue.Default , transport[i] , topic[i] , null);
                    listeners[i].MessageReceived += new MessageReceivedEventHandler(callBack[i]);
                    Console.WriteLine("Listening on: " + topic[i]);
                } catch (RendezvousException exception) {
                    Console.Error.WriteLine("Failed to create listener:");
                    Console.Error.WriteLine(exception.StackTrace);
                    System.Environment.Exit(1);
                }
            }

            // dispatch Rendezvous events
            while (true) {
                try {
                    //Console.WriteLine(Queue.Default.Count);//
                    Queue.Default.Dispatch();
                } catch (RendezvousException exception) {
                    Console.Error.WriteLine("Exception dispatching default queue:");
                    Console.Error.WriteLine(exception.StackTrace);
                    break;
                }
            }

            // Force optimizer to keep alive listeners up to this point.
            GC.KeepAlive(listeners);

            TIBCO.Rendezvous.Environment.Close();

        }
    }

    /// <summary>
    /// Send via TIBCO Rendezvous service
    /// </summary>
    public class TIBCORVSender
    {
        private Transport transport = null;

        /// <summary>
        /// Initiate sender with parameters
        /// </summary>
        /// <param name="service">service parameter</param>
        /// <param name="network">network parameter</param>
        /// <param name="daemon">daemon parameter</param>
        public TIBCORVSender(string service , string network , string daemon) {
            try {
                /* Create internal TIB/Rendezvous machinery */
                if (TIBCO.Rendezvous.Environment.IsIPM()) {
                    TIBCO.Rendezvous.Environment.Open(".\\tibrvipm.cfg");
                } else {
                    TIBCO.Rendezvous.Environment.Open();
                }
            } catch (RendezvousException exception) {
                Console.Error.WriteLine("Failed to open Rendezvous Environment: {0}" , exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
                Console.Error.WriteLine("Press any key to exit.");
                Console.ReadKey();
                System.Environment.Exit(1);
            }

            // Create Network transport            
            try {
                transport = new NetTransport(service , network , daemon);
            } catch (RendezvousException exception) {
                Console.Error.WriteLine("Failed to create NetTransport:");
                Console.Error.WriteLine(exception.StackTrace);
                Console.Error.WriteLine("Press any key to exit.");
                Console.ReadKey();
                System.Environment.Exit(1);
            }


        }

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">Message to be send.</param>
        /// <param name="topic">Send message through topic.</param>
        public void Send(Message message , string topic) {

            // Set send subject into the message
            try {
                message.SendSubject = topic;
            } catch (RendezvousException exception) {
                Console.Error.WriteLine("Failed to set send subject:");
                Console.Error.WriteLine(exception.StackTrace);
                Console.Error.WriteLine("Press any key to exit.");
                Console.ReadKey();
                System.Environment.Exit(1);
            }

            try {
                //message.AddField("DATA" , Msg , 0);
                transport.Send(message);
            } catch (RendezvousException exception) {
                Console.Error.WriteLine("Error sending a message:");
                Console.Error.WriteLine(exception.StackTrace);
                Console.Error.WriteLine("Press any key to exit.");
                Console.ReadKey();
                System.Environment.Exit(1);
            }

        }
    }
}
