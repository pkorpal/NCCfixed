using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NCC
{
    class CallRequest
    {
        String source;
        String destination;
        int connectionId;
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
            destination = smsg[4];
            throughput = Int16.Parse(smsg[6]);
            policy.checkForPolicyIssue(source, destination, throughput);   
            Console.WriteLine("Setting up connection between {0} and {1}", this.source, this.destination);
        }

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

        public string getPath()
        {
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

        public bool resourceAllocation()
        {
            return false;
        }

        public double getCallRequestThroughput()
        {
            return this.throughput;
        }
    }
}
