using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace NCC
{
    class CallRequest
    {
        String source;
        //String source_snpp;
        String destination;
        //String destination_snpp;
        int connectionId;
        //List<string> path = new List<string>();
        int throughput;

        Policy policy;
        Directory directory;

        public CallRequest(string msg, int connectionId)
        {
            policy = new Policy();
            directory = new Directory();
            this.connectionId = connectionId;
            string[] smsg = msg.Split(' ');
            source = smsg[2];
            //Console.WriteLine("Source: {0}", source);
            //source_snpp = directory.getSnpp(source);
            //Console.WriteLine("Source snpp: {0}", source_snpp);
            destination = smsg[4];
            //Console.WriteLine("Destination: {0}", destination);
            //destination_snpp = directory.getSnpp(destination);
            //Console.WriteLine("Destination snpp: {0}", destination_snpp);
            throughput = Int16.Parse(smsg[6]);
            //Console.WriteLine("Throughput: {0}", throughput);
            //path.Add(source_snpp);
            policy.checkForPolicyIssue(source, destination, throughput);   
            Console.WriteLine("Setting up connection between {0} and {1}", this.source, this.destination);
        }

        //public string getNextHop() {
        //    string last_snpp = path[path.Count-1];
        //    return directory.getNextSnpp(last_snpp);
        //    return directory.getEdgeNode(source);
        //}


        public int getDevicePort(string device)
        {
            RouterConnection rc = new RouterConnection();
            rc.getConnectedRouters();
            return rc.getDevicePort(device);
        }

        public bool checkIfPathComplete(string path) {
            string[] pathElements = path.Split(' ');
            if (pathElements[pathElements.Length - 1] == destination)
                return true;
            else
                return false;
        }

        public string SendCallCoordinationRequest(string source, string destination, int throughput, int NCCport)
        {
            string msg = "CALL_COORDINATION_REQUEST SOURCE " + source + " DESTINATION " + destination + " THROUGHPUT " + throughput;
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            Socket socket = new Socket(IPAddress.Loopback.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveBufferSize = 256;
            socket.SendBufferSize = 256;
            socket.DontFragment = true;
            socket.NoDelay = true;
            socket.SendTimeout = 500;
            socket.LingerState = new LingerOption(true, 2);

            socket.Connect(IPAddress.Loopback, NCCport);
            socket.Send(bytes);
            Byte[] bytesRec = new Byte[256];
            socket.Receive(bytesRec);
            string response = Encoding.ASCII.GetString(bytesRec);
            Console.WriteLine(response);
            response.Replace("PATH ", "");
            return response;
        }

        public string getLastPathElement(string path)
        {
            string[] spath = path.Split(' ');
            string lastNode = "";
            try
            {
                lastNode = spath[spath.Length - 1];
            } catch(Exception e)
            {
                Console.WriteLine("CallRequest.getLast: empty path");
                Console.WriteLine(e);
            }
            return spath[spath.Length - 1];
        }

        public string getPath() {
            //string snpp = directory.getSnpp(source);
            //string nexthop = directory.getNextSnpp(snpp);
            string device = directory.getEdgeNode(source);
            int port = getDevicePort(device);
            RouterConnection rc = new RouterConnection();
            string path = rc.sendToRouter(port, source, destination, connectionId);
            while (!checkIfPathComplete(path))
            {
                string lastNode = getLastPathElement(path);
                string nextNCC = directory.getAdjacentSubnetwork(lastNode);
                string adjacentSubnetworkPath = SendCallCoordinationRequest(lastNode, destination, throughput, Int32.Parse(nextNCC));
                path += adjacentSubnetworkPath;
            }
            return path;
        }
    }
}
