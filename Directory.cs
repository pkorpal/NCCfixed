using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace NCC
{
    class Directory
    {
        Dictionary<string, string> edgeNodes = new Dictionary<string, string>();

        Dictionary<string, string> adjacentSubnetworks = new Dictionary<string, string>();

        List<string> subdomainSnppList = new List<string>();

        public Directory() {
            setupDirectory();
        }

        public string getAdjacentSubnetwork(string snpp) {
            string adjacentNCC = "";
            try
            {
                adjacentNCC = adjacentSubnetworks[snpp];
            } catch (Exception e)
            {
                Console.WriteLine("Directory.getAdjacentSubnetwork: Cannot find adjacent NCC for snpp {0}", snpp);
                Console.WriteLine(e);
            }
            return adjacentNCC;
        }

        public string getEdgeNode(string device)
        {
            string d = "";
            try
            {
                d = edgeNodes[device];
            } catch(Exception e)
            {
                Console.WriteLine("Directory.getEdgeNodePort: Cannot find edge node for device {0}", device);
                Console.WriteLine(e);
            }
            return d;
        }

        public void setupDirectory() {
            string path = System.IO.Directory.GetCurrentDirectory() + "/directory.txt";
            string[] config = System.IO.File.ReadAllLines(path);

            foreach(var line in config) {
                string[] configEntry = line.Split(' ');
                if (configEntry[0] == "ADJACENT_SUBNETWORS")
                    adjacentSubnetworks.Add(configEntry[1], configEntry[2]);
                else if (configEntry[0] == "EDGE_NODES")
                    edgeNodes.Add(configEntry[1], configEntry[2]);
                else
                    continue;
            }
            showAdjacentNetworks();
            showEdgeNodes();

        }

        public void showEdgeNodes()
        {
            Console.WriteLine("Edge Nodes in network");
            foreach (var en in this.edgeNodes)
                Console.WriteLine("EDGE NODE: {0} CONNECTED TO: {1}", en.Key, en.Value);
        }

        public void showSubdomainSnppList() {
            Console.WriteLine("Snpps in subdomain");
            foreach(var snpp in this.subdomainSnppList)
                Console.WriteLine("SNPP: {0}", snpp);
        }

        public void showAdjacentNetworks() {
            Console.WriteLine("Adjacent networks");
            foreach(var network in this.adjacentSubnetworks)
                Console.WriteLine("NODE: {0} NCC: {1}", network.Key, network.Value);
        }
    }
}
