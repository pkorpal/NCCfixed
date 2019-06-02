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
        String destination;
        String path = "";
        int throughput;
        private bool running = false;

        Policy policy;
        Directory directory;

        public CallRequest(string msg)
        {
            this.message = msg;
            string[] smsg = msg.Split(' ');
            source = smsg[2];
            destination = smsg[4];
            throughput = Int16.Parse(smsg[6]);
            path += source + " ";
            policy = new Policy();
            directory = new Directory();
            Console.WriteLine("Setting up connection between {0} and {1}", this.source, this.destination);
        }

        public string getNextHop() {
            return "";
        }

        public bool checkIfPathComplete() {
            return false;
        }
    }
}
