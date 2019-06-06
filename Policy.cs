using System;

namespace NCC
{
    class Policy
    {
        public Policy() { }

        public void checkForPolicyIssue(string sender, string receiver, int capacity)
        {
            Console.WriteLine("[POLICY]: Policy request, sender {0}, receiver {1}, capacity {2}", sender, receiver, capacity);
            Console.WriteLine("[POLICY]: Processing request");
            Console.WriteLine("[POLICY]: Accepted");
        }
    }
}
