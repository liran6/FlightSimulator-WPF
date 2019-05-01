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
    class ConnectionCommand
    {
        private bool isConnected;
        private static ConnectionCommand instance = null;
        private Thread thread;
        private NetworkStream networkStream;
        private TcpListener serverSide;
        private TcpClient client;
        private readonly static object mut = new object();


        public ConnectionCommand()
        {
            this.isConnected = false;
        }

        public static ConnectionCommand getInstance
        {
            get
            {
                return instance == null ? instance = new ConnectionCommand() : instance;
            }
        }

      public void ConnectToServer()
        {
            this.thread = new Thread(() =>
            {
                try { 
                IPEndPoint eP = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                   Properties.Settings.Default.FlightCommandPort);
                this.client = new TcpClient();
                this.serverSide = new TcpListener(eP);
                client.Connect(eP);
                    }
                    catch (Exception)
                    {
                    }              
                isConnected = true;
                this.networkStream = client.GetStream();
            });
            thread.Start();
        }
        /*
        *sending commands from the auto pilot.
        */
        public void AutoSend(string[] commands)
        {
            if (!isConnected) return;

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

        public void JoystickSend(string path, double val)
        {
            if (!isConnected)
            {
                return;
            }
            string strVal = val.ToString("0.00");
            string command = "set " + path + " " + strVal + "\r\n";        
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(command.ToString());
            networkStream.Write(buffer, 0, buffer.Length);       
            networkStream.Flush();              
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
