﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace NCC
{
    class CallCoordinationRequest
    {
        String message;
        String source;
        String source_snpp;
        String destination;
        String destination_snpp;
        int connectionId;
        List<string> path = new List<string>();
        int throughput;

        Policy policy;
        Directory directory;

        public CallCoordinationRequest(string msg, int connectionId)
        {
            policy = new Policy();
            directory = new Directory();
            this.connectionId = connectionId;
            this.message = msg;
            string[] smsg = msg.Split(' ');
            source = smsg[2];
            Console.WriteLine("Source: {0}", source);
            source_snpp = directory.getSnpp(source);
            Console.WriteLine("Source snpp: {0}", source_snpp);
            destination = smsg[4];
            Console.WriteLine("Destination: {0}", destination);
            destination_snpp = directory.getSnpp(destination);
            throughput = Int16.Parse(smsg[6]);
            path.Add(source_snpp);
            policy.checkForPolicyIssue(source, destination, throughput);
            Console.WriteLine("Setting up connection between {0} and {1}", this.source, this.destination);
        }

        public string getNextHop()
        {
            string last_snpp = path[path.Count - 1];
            return directory.getNextSnpp(last_snpp);
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
            string snpp = directory.getSnpp(source);
            string nexthop = directory.getNextSnpp(snpp);
            string device = directory.getDeviceName(nexthop);
            int port = getDevicePort(device);
            RouterConnection rc = new RouterConnection();
            rc.sendToRouter(port, source, destination, connectionId);
            return "";
        }
    }
}
