﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace NCC
{

    class Handler
    {
        Socket socket;
        Byte[] bytes;
        String data;
        int connectionId;
        public Handler(Socket s, int connectionId)
        {
            this.socket = s;
            this.bytes = new Byte[100];
            this.connectionId = connectionId;
            start();
        }

        private void start()
        {
            Thread t = new Thread(new ThreadStart(run));
            t.Start();
        }
        private void run()
        {
            int byteREc = socket.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes, 0, byteREc);
            Console.WriteLine(data);
            if (data.IndexOf("CALL_REQUEST") > -1)
            {
                CallRequest callRequest = new CallRequest(data, connectionId);
                string path = callRequest.getPath();
                string msg = "CONNECTION SET";
                byte[] message = Encoding.UTF8.GetBytes(msg); 
                socket.Send(message);
            } else if (data.IndexOf("CALL_COORDINATION_REQUEST") > -1) {
                CallCoordinationRequest callCoordinationRequest = new CallCoordinationRequest(data, connectionId);
                string path = callCoordinationRequest.getPath();
                byte[] response = callCoordinationRequest.getCallCoordinationResponse(path);
                socket.Send(response);
            } else if (data.IndexOf("CONNECT") > -1) {
                string[] smsg = data.Split(' ');
                RouterConnection routerConnection = new RouterConnection();
                routerConnection.setRouterDetails(smsg[2], smsg[4]);
                routerConnection.getConnectedRouters();
                routerConnection.updateConnectedRouters();
            }
        }
    }
}