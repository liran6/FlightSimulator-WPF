using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    /**
     * class info incharge of the connection of the simlator to the flightgear and the info transfermation between them.
     */
   public class Info
    {
        private float lon;
        private float lat;
        private bool stop;
        private static Info instance = null;
        Thread thread;
        TcpClient client;
        TcpListener serverSide;
        BinaryReader reader;
        NetworkStream stream;


        // constructor to initialize 
        public Info()
        {
            lon = 0.0f;
            lat = 0.0f;
            stop = false;
        }

        // Property of Lon
        public float Lon
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
            }
        }

        // Property of Lat
        public float Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }

        /*
* singleton create only one instance of info
*/
        public static Info getInstance
        {
            get
            {
                return instance == null ? instance = new Info() : instance;
            }
        }

        /*
 * open & connect to the server , send data from the simulator.
 */
        public void startServer()
        {
            IPEndPoint eP = new IPEndPoint(IPAddress.Parse(Properties.Settings.Default.FlightServerIP),
                                                      Properties.Settings.Default.FlightInfoPort);
            this.serverSide = new TcpListener(eP);
            this.stream = this.client.GetStream();
            this.reader = new BinaryReader(stream);
            String[] splitInput;

            // opens server
            this.serverSide.Start();
            this.client = serverSide.AcceptTcpClient();
            
            while (!stop)
            {
                // read the input fron the simulator
                string input = "";
                char c;

                // read data untill \n
                while ((c = reader.ReadChar()) != '\n')
                {
                    input += c;
                }

                // splits the input
                splitInput = input.Split(',');

                // gets the lon and lat from the input and add them to the lon and lat in the instance
                FlightBoardViewModel.getInstance.Lon = float.Parse(splitInput[0]);
                FlightBoardViewModel.getInstance.Lat = float.Parse(splitInput[1]);

            }
        }
        public bool isServerOpen()
        {
            return this.serverSide != null;
        }
        /*
 * activate the startServer from a thread
 */
        public void openServer()
        {
            this.thread = new Thread(() => startServer());
            thread.Start();
        }

        public void closeServer()
        {
            this.stop = true;
            this.thread.Abort();
            this.client.Close();
            this.serverSide.Stop();
        }
    }
}
