using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NCC
{
    class Directory
    {
        Dictionary<string, string> clients = new Dictionary<string, string>();

        Dictionary<string, string> snppDict = new Dictionary<string, string>();

        Dictionary<string, string> devices = new Dictionary<string, string>();

        Dictionary<string, string> adjacentSubnetworks = new Dictionary<string, string>();

        List<string> subdomainSnppList = new List<string>();

        public Directory()
        {
            setupDirectory();
        }

        public void addSnnpConnections(string key, string value)
        {
            snppDict.Add(key, value);
        }

        public void addSubdomainSnpp(string snpp)
        {
            subdomainSnppList.Add(snpp);
        }

        public string getNextSnpp(string snpp)
        {
            return snppDict[snpp];
        }

        public string getDeviceName(string snpp)
        {
            return devices[snpp];
        }

        public bool checkIfSnppInSubdomain(string snpp)
        {
            bool check = false;
            foreach (string s in subdomainSnppList)
                if (s == "snpp")
                    check = true;
            return check;
        }

        public string getAdjacentSubnetwork(string snpp)
        {
            return adjacentSubnetworks[snpp];
        }

        public string getSnpp(string client)
        {
            return clients[client];
        }

        public void setupDirectory()
        {
            Console.WriteLine("Setting up directory");
            using (StreamReader sr = new StreamReader("directory.json"))
            {
                var json = sr.ReadToEnd();
                var tmp = JsonConvert.DeserializeObject<Directory>(json);
                this.clients = tmp.clients;
                this.snppDict = tmp.snppDict;
                this.devices = tmp.devices;
                this.adjacentSubnetworks = tmp.adjacentSubnetworks;
                this.subdomainSnppList = tmp.subdomainSnppList;
            }
            showClients();
            showDevices();
            showSubdomainSnppList();
            showAdjacentNetworks();
            showSnppConnections();
        }

        public void showClients()
        {
            Console.WriteLine("Clients in network");
            foreach (var client in this.clients)
                Console.WriteLine("Client: {0}, Snpp: {1}", client.Key, client.Value);
        }

        public void showDevices()
        {
            Console.WriteLine("Devices in subdomain");
            foreach (var device in this.devices)
                Console.WriteLine("Snpp: {0}, Device: {1}", device.Key, device.Value);
        }

        public void showSubdomainSnppList()
        {
            Console.WriteLine("Snpps in subdomain");
            foreach (var snpp in this.subdomainSnppList)
                Console.WriteLine("Snpp: {0}", snpp);
        }

        public void showAdjacentNetworks()
        {
            Console.WriteLine("Adjacent networks");
            foreach (var network in this.adjacentSubnetworks)
                Console.WriteLine("Snpp: {0}, NCC: {1}", network.Key, network.Value);
        }

        public void showSnppConnections()
        {
            Console.WriteLine("Snpp connections");
            foreach (var sc in this.snppDict)
                Console.WriteLine("Source: {0} Destination: {1}");
        }
    }
}
