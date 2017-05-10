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
    public delegate void ListenerFunc(object listener, MessageReceivedEventArgs messageReceivedEventArgs);

    /// <summary>
    /// Listen to TIBCO Rendezvous services. This class can let you specify multiple net transports and listener callbacks.
    /// </summary>
    public class TIBCORVListener
    {
        private int N = 0;
        private Transport[] transport;
        private string[] topics;

        /// <summary>
        /// Initiate listener with parameters
        /// </summary>
        /// <param name="services">String array of service parameters</param>
        /// <param name="networks">String array of network parameters</param>
        /// <param name="daemons">String array of daemon parameters</param>
        /// <param name="topics">String array of topic parameters</param>
        /// <exception cref="ArgumentException">Parameter arrays must have same length</exception>
        public TIBCORVListener(string[] services, string[] networks, string[] daemons, string[] topics) {
            N = services.Length;
            if (N != networks.Length || N != daemons.Length || N != topics.Length)
                throw new ArgumentException("Parameter arrays must have same length");

            this.topics = new string[N];
            for (int i = 0; i < N; i++)
                this.topics[i] = topics[i];

            Constructor(services, networks, daemons);           
        }

        /// <summary>
        /// Initiate listener with parameters
        /// </summary>
        /// <param name="service">String of service parameter</param>
        /// <param name="network">String of network parameter</param>
        /// <param name="daemon">String of daemon parameter</param>
        /// <param name="topic">String of topic parameter</param>
        public TIBCORVListener(string service, string network, string daemon, string topic) {
            N = 1;
            topics = new string[] { topic };
            Constructor(new string[] { service }, new string[] { network }, new string[] { daemon });
        }

        /// <summary>
        /// Initiate listener with RVParameters classes
        /// </summary>
        /// <param name="rvParameters">RVParameters classes</param>
        public TIBCORVListener(RVParameters[] rvParameters) {
            N = rvParameters.Length;
            string[] services = new string[N];
            string[] networks = new string[N];
            string[] daemons = new string[N];
            topics = new string[N];
            for (int i = 0; i < N; i++) {
                services[i] = rvParameters[i].service;
                networks[i] = rvParameters[i].network;
                daemons[i] = rvParameters[i].daemon;
                topics[i] = rvParameters[i].topic;
            }
            Constructor(services, networks, daemons);
        }
        /// <summary>
        /// Initiate listener with RVParameters class
        /// </summary>
        /// <param name="rvParameter">RVParameters class</param>
        public TIBCORVListener(RVParameters rvParameter) {
            N = 1;
            topics = new string[] { rvParameter.topic };
            Constructor(new string[] { rvParameter.service }, new string[] { rvParameter.network }, new string[] { rvParameter.daemon });
        }
        private void Constructor(string[] services, string[] networks, string[] daemons) {
            transport = new Transport[N];
            try {
                /* Create internal TIB/Rendezvous machinery */
                if (TIBCO.Rendezvous.Environment.IsIPM()) {
                    TIBCO.Rendezvous.Environment.Open(".\\tibrvipm.cfg");
                } else {
                    TIBCO.Rendezvous.Environment.Open();
                }
            } catch (RendezvousException exception) {
                Console.Error.WriteLine("Failed to open Rendezvous Environment: {0}", exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
                Console.ReadKey();
                System.Environment.Exit(1);
            }
            Console.WriteLine("Opened Environment"); //
            for (int i = 0; i < N; i++) {
                // Create Network transport
                try {
                    transport[i] = new NetTransport(services[i], networks[i], daemons[i]);
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
        /// <param name="callBack">Callback functions to be called on reveiviing message.</param>
        /// <exception cref="ArgumentException">Parameter arrays must have length N equals to lengths of parameters of constructor</exception>
        public void Listen(ListenerFunc[] callBack) {
            if (N != callBack.Length)
                throw new ArgumentException("Parameter arrays must have length " + N);

            Listener[] listeners = new Listener[N];
            for (int i = 0; i < N; i++) {
                // Create listeners for specified subjects                
                try {
                    listeners[i] = new Listener(Queue.Default, transport[i], topics[i], null);
                    listeners[i].MessageReceived += new MessageReceivedEventHandler(callBack[i]);
                    Console.WriteLine("Listening on: " + topics[i]);
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

        /// <summary>
        /// Start listen to topic with callback function.
        /// </summary>
        /// <param name="callBack">Callback function to be called on reveiviing message.</param>
        /// /// <exception cref="ArgumentException">Parameter arrays must have length N equals to lengths of parameters of constructor</exception>
        public void Listen(ListenerFunc callBack) {
            if (N != 1)
                throw new ArgumentException("Parameter arrays must have length " + N +", try another overload instead.");
            Listen(new ListenerFunc[] { callBack });
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
        public TIBCORVSender(string service, string network, string daemon) {
            try {
                /* Create internal TIB/Rendezvous machinery */
                if (TIBCO.Rendezvous.Environment.IsIPM()) {
                    TIBCO.Rendezvous.Environment.Open(".\\tibrvipm.cfg");
                } else {
                    TIBCO.Rendezvous.Environment.Open();
                }
            } catch (RendezvousException exception) {
                Console.Error.WriteLine("Failed to open Rendezvous Environment: {0}", exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
                Console.Error.WriteLine("Press any key to exit.");
                Console.ReadKey();
                System.Environment.Exit(1);
            }

            // Create Network transport            
            try {
                transport = new NetTransport(service, network, daemon);
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
        public void Send(Message message, string topic) {

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
