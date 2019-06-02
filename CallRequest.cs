using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace NCC
{
    class CallRequest
    {
        String message;
        String source;
        String source_snpp;
        String destination;
        String destination_snpp;
        List<string> path = List<string>();
        int throughput;
        private bool running = false;

        Policy policy;
        Directory directory;

        public CallRequest(string msg)
        {
            this.message = msg;
            string[] smsg = msg.Split(' ');
            source = smsg[2];
            source_snpp = directory.getSnpp(source);
            destination = smsg[4];
            destination_snpp = directory.getSnpp(destination);
            throughput = Int16.Parse(smsg[6]);
            path.Add(source_snpp);
            policy = new Policy();
            directory = new Directory();
            Console.WriteLine("Setting up connection between {0} and {1}", this.source, this.destination);
        }

        public string getNextHop() {
            string last_snpp = path[path.Count-1];
            return directory.getNextSnpp(last_snpp);
        }

        public bool checkIfPathComplete() {
            return false;
        }

        public void getPath() {
            while(path)
        }
    }
}
