using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace NCC
{
    class Policy
    {
        public Policy() { }

        public void policy(string sender, string receiver, int capacity)
        {
            Console.WriteLine("[POLICY]: Policy request, sender {0}, receiver {1}, capacity {2}", sender, receiver, capacity);
            Console.WriteLine("[POLICY]: Processing request");
            Console.WriteLine("[POLICY]: Accepted");
        }
    }
}
