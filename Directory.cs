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
        Dictionary<string, string> clients = new Dictionary<string, string>();

        Dictionary<string, string> snppDict = new Dictionary<string, string>();

        Dictionary<string, string> devices = new Dictionary<string, string>();

        Dictionary<string, string> adjacentSubnetworks = new Dictionary<string, string>();

        List<string> subdomainSnppList = new List<string>();

        public Directory() {
            setupDirectory();
            //showClients();
            //showDevices();
            //showAdjacentNetworks();
            //showSnppConnections();
            //showSubdomainSnppList();
        }

        public void addSnnpConnections(string key, string value) {
            snppDict.Add(key, value);
        }

        public void addSubdomainSnpp(string snpp) {
            subdomainSnppList.Add(snpp);
        }

        public string getNextSnpp(string snpp) {
            string nextSnpp = "";
            try
            {
                nextSnpp = snppDict[snpp];
            }
            catch (Exception e)
            {
                Console.WriteLine("Directory.getNextSnpp: Cannot find next snpp for {0}", snpp);
                Console.WriteLine(e);
            }
            return nextSnpp;
        }

        public string getDeviceName(string snpp) {
            string deviceName = ""; 
            try
            {
                deviceName = devices[snpp];
            } catch (Exception e)
            {
                Console.WriteLine("Directory.getDeviceName: Cannot find device with snpp {0}", snpp);
                Console.WriteLine(e);
            }
            return deviceName;
        }

        public bool checkIfSnppInSubdomain(string snpp) {
            bool check = false;
            foreach(string s in subdomainSnppList)
                if(s == "snpp")
                    check = true;
            return check;
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

        public string getSnpp(string client) {
            string snpp = "";
            try
            {
                snpp = clients[client];
            } catch (Exception e)
            {
                Console.WriteLine("Directory.getSnpp: Cannot find client with snpp {0}", client);
                Console.WriteLine(e);
            }
            return snpp;
        }

        public void setupDirectory() {
            string path = System.IO.Directory.GetCurrentDirectory() + "/directory.txt";
            string[] config = System.IO.File.ReadAllLines(path);

            foreach(var line in config) {
                string[] configEntry = line.Split(' ');
                if(configEntry[0] == "CLIENTS")
                    clients.Add(configEntry[1], configEntry[2]);
                else if(configEntry[0] == "SUBDOMAIN_SNPP_LIST")
                    subdomainSnppList.Add(configEntry[1]);
                else if(configEntry[0] == "ADJACENT_SUBNETWORS")
                    adjacentSubnetworks.Add(configEntry[1], configEntry[2]);
                else if(configEntry[0] == "DEVICES")
                    devices.Add(configEntry[1], configEntry[2]);
                else if(configEntry[0] == "SNPP_DICT")
                    snppDict.Add(configEntry[1], configEntry[2]);
                else
                    continue;
            }
            showClients();
            showDevices();
            showAdjacentNetworks();
            showSnppConnections();
            showSubdomainSnppList();

        }

        public void showClients() {
            Console.WriteLine("Clients in network");
            foreach(var client in this.clients)
                Console.WriteLine("CLIENT: {0} SNPP: {1}", client.Key, client.Value);
        }

        public void showDevices() {
            Console.WriteLine("Devices in subdomain");
            foreach(var device in this.devices)
                Console.WriteLine("SNPP: {0} DEVICE: {1}", device.Key, device.Value);
        }

        public void showSubdomainSnppList() {
            Console.WriteLine("Snpps in subdomain");
            foreach(var snpp in this.subdomainSnppList)
                Console.WriteLine("SNPP: {0}", snpp);
        }

        public void showAdjacentNetworks() {
            Console.WriteLine("Adjacent networks");
            foreach(var network in this.adjacentSubnetworks)
                Console.WriteLine("SNPP: {0} NCC: {1}", network.Key, network.Value);
        }

        public void  showSnppConnections() {
            Console.WriteLine("Snpp connections");
            foreach(var sc in this.snppDict)
                Console.WriteLine("SOURCE: {0} DESTINATION: {1}", sc.Key, sc.Value);
        }
    }
}
