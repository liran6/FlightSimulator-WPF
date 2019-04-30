using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace FlightSimulator.Model
{
    class Command
    {
        private bool isConnected;
        private static Command instance = null;
        private Thread thread;
        //private Thread sendThread;
        private NetworkStream networkStream;
        private TcpListener serverSide;
        private TcpClient client;
        private readonly static object mut = new object();


        public Command()
        {
            this.isConnected = false;
        }

        public static Command getInstance
        {
            get
            {
                return instance == null ? instance = new Command() : instance;
            }
        }

        /*
    * open client and connect to the server
    */
        void connectToServer()
        {
            IPEndPoint eP = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                  Properties.Settings.Default.FlightCommandPort);
            this.client = new TcpClient();
            this.serverSide = new TcpListener(eP);

            // connect to server
            while (!client.Connected)
            {
                try
                {
                    client.Connect(eP);
                }
                catch (Exception)
                {
                }
            }

            isConnected = true;
            this.networkStream = client.GetStream();
        }
        /*
        *sending commands from the auto pilot.
        */
        public void sendToSimulator(string userCommands)
        {
            if (isConnected) return;

            // split command by the current environment
            string[] commands = userCommands.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Thread t = new Thread(() =>
            {
                foreach (string command in commands)
                {
                    string cmd = command + "\r\n";
                    lock (mut)
                    {
                        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(cmd.ToString());
                        networkStream.Write(buffer, 0, buffer.Length);
                        networkStream.Flush();
                    }
                    Thread.Sleep(2000);
                }
            });
            t.Start();
        }

        public void JoystickSendToSimulator(string command)
        {
            if (!isConnected)
            {
                return;
            }
        
            string line = command + "\r\n";
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(line.ToString());
            networkStream.Write(buffer, 0, buffer.Length);       
            networkStream.Flush();              
        }


        /*
 * activate the connectToServer from a thread
 */
        public void startClient()
        {
            this.thread = new Thread(() => connectToServer());
            thread.Start();
        }

        /*
   * close the connection to the client, and the server that are connected to him
   */
        public void closeClient()
        {
            this.isConnected = false;
            if (this.thread != null)
            {
                this.thread.Abort();
            }
            this.thread.Abort();
            this.client.Close();
            this.serverSide.Stop();
        }

        public bool isOpen()
        {
            return null != this.client;
        }
    }
}
