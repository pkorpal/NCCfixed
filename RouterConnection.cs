using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NCC
{
    class RouterConnection
    {

        string routerName;
        string routerPort;
        Dictionary<string, string> connectedRouters = new Dictionary<string, string>();
        string path = System.IO.Directory.GetCurrentDirectory() + "/connectedRouters.txt";

        public RouterConnection() {}

        public void setRouterDetails(string rn, string rp)
        {
            this.routerPort = rp;
            this.routerName = rn;
        }

        public void updateConnectedRouters()
        {
            try
            {
                connectedRouters.Add(routerName, routerPort);
                System.IO.File.WriteAllText(path, String.Empty);
                string s = GetLine(connectedRouters);
                System.IO.File.WriteAllText(path, s);
            } catch (Exception e)
            {
                Console.WriteLine("RouterConnection.updateConnectedRouters: Error while updating connected routers");
                Console.WriteLine(e);
            }
        }

        string GetLine(Dictionary<string, string> d)
        {
            // Build up each line one-by-one and then trim the end
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in d)
            {
                builder.Append(pair.Key).Append(" ").Append(pair.Value).Append('\n');
            }
            string result = builder.ToString();
            // Remove the final delimiter
            result = result.TrimEnd(',');
            return result;
        }

        public void getConnectedRouters()
        {
            string[] config = System.IO.File.ReadAllLines(path);
            for (int i = 0; i < config.Length; i++)
            {
                string[] entry = config[i].Split(' ');
                connectedRouters.Add(entry[0], entry[1]);
            }
        }

        public void showConnectedRouters()
        {
            foreach (var entry in connectedRouters)
            {
                Console.WriteLine("Router: {0} Port: {1}", entry.Key, entry.Value);
            }
        }

        public string sendToRouter(int port, string sender, string destination, int connectionId)
        {
            Socket socket = new Socket(IPAddress.Loopback.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveBufferSize = 256;
            socket.SendBufferSize = 256;
            socket.DontFragment = true;
            socket.NoDelay = true;
            socket.SendTimeout = 500;
            socket.LingerState = new LingerOption(true, 2);

            socket.Connect(IPAddress.Loopback, port);

            Byte[] bytes = new Byte[256];
            string msg = "PATH_REQUEST SOURCE " + sender + " DESTINATION " + destination + " CONNECTION_ID " + connectionId;
            bytes = Encoding.UTF8.GetBytes(msg);
            socket.Send(bytes);
            Byte[] response = new Byte[256];
            socket.Receive(response);
            string sresponse = Encoding.ASCII.GetString(response);
            Console.WriteLine(sresponse);
            return sresponse;
        }

        public int getDevicePort(string device)
        {
            return Int32.Parse(connectedRouters[device]);
        }
    }
}
