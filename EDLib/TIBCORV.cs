using System;
using TIBCO.Rendezvous;

namespace EDLib.TIBCORV
{
    /// <summary>
    /// Function pointer of listener callback function
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="messageReceivedEventArgs"></param>
    public delegate void ListenerFunc(object listener , MessageReceivedEventArgs messageReceivedEventArgs);

    /// <summary>
    /// Listen to TIBCO Rendezvous service
    /// </summary>
    public class TIBCORVListener
    {
        int N = 0;
        private Transport[] transport;

        /// <summary>
        /// Initiate listener with parameters
        /// </summary>
        /// <param name="service"></param>
        /// <param name="network"></param>
        /// <param name="daemon"></param>
        public TIBCORVListener(string[] service , string[] network , string[] daemon) {

            N = service.Length;
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
            Console.WriteLine("Open Environment"); //
            for (int i = 0; i < N; i++) {
                // Create Network transport
                try {
                    transport[i] = new NetTransport(service[i] , network[i] , daemon[i]);
                    Console.WriteLine("Create Net Transport"); //
                } catch (RendezvousException exception) {
                    Console.Error.WriteLine("Failed to create NetTransport");
                    Console.Error.WriteLine(exception.StackTrace);
                    Console.ReadKey();
                    System.Environment.Exit(1);
                }
            }
        }

        /// <summary>
        /// Start listen to topics with callback functions
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="CallBack"></param>
        public void Listen(string[] topic , ListenerFunc[] CallBack) {
            Listener[] listeners = new Listener[N];
            for (int i = 0; i < N; i++) {
                // Create listeners for specified subjects                
                try {
                    listeners[i] = new Listener(Queue.Default , transport[i] , topic[i] , null);
                    listeners[i].MessageReceived += new MessageReceivedEventHandler(CallBack[i]);
                    Console.Error.WriteLine("Listening on: " + topic[i]);
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
        /// <param name="service"></param>
        /// <param name="network"></param>
        /// <param name="daemon"></param>
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
        /// <param name="message"></param>
        /// <param name="topic"></param>
        public void Send(Message message , string topic) {
            // Create the message
            //Message message = new Message();

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
