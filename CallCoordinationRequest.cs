using System;
using System.Text;

namespace NCC
{
    class CallCoordinationRequest
    {
        String source;
        String destination;
        int connectionId;
        int throughput;

        Policy policy;
        Directory directory;

        public CallCoordinationRequest(string msg, int connectionId)
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

        public byte[] getCallCoordinationResponse(string path)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(path);
            return bytes;
        }

        public int getDevicePort(string device)
        {
            RouterConnection rc = new RouterConnection();
            rc.getConnectedRouters();
            return rc.getDevicePort(device);
        }

        public string getPath()
        {
            string device = directory.getEdgeNode(source);
            int port = getDevicePort(device);
            RouterConnection rc = new RouterConnection();
            string path = rc.sendToRouter(port, source, destination, connectionId);
            return "";
        }
    }
}
