using System;
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
            if (data.IndexOf("CALL_REQUEST") > -1) // NCC receives from client a request to setup a connection with another client
            {
                CallRequest callRequest = new CallRequest(data, connectionId);
                string path = callRequest.getPath();
                if (path != "")
                {
                    Console.WriteLine("Calculating slots for connection");
                    ModulationTable mt = new ModulationTable();
                    int distance = 100;
                    string mod = mt.getModulation(distance);
                    int parameter = mt.getModulationMultiplier(mod);
                    SlotsCalculator sc = new SlotsCalculator();
                    double throughput = callRequest.getCallRequestThroughput();
                    int slots = sc.calculateSlots(parameter, throughput);

                    // make request to edge router to allocate resources

                    string msg = "CONNECTION SET"; // 
                    byte[] message = Encoding.UTF8.GetBytes(msg);
                    socket.Send(message);
                }
                else
                {
                    string msg = "CONNECTION FAILED";
                    byte[] message = Encoding.UTF8.GetBytes(msg);
                    socket.Send(message);
                }

            }
            else if (data.IndexOf("CALL_COORDINATION_REQUEST") > -1) // NCC receives from adjacent CALL COORDINATION REQUEST to setup a connection between two clients
            {
                CallCoordinationRequest callCoordinationRequest = new CallCoordinationRequest(data, connectionId);
                string path = callCoordinationRequest.getPath();
                if (path != "")
                {
                    byte[] response = callCoordinationRequest.getCallCoordinationResponse(path);
                    socket.Send(response);
                }
                else
                {
                    string msg = "CONNECTION FAILED";
                    byte[] message = Encoding.UTF8.GetBytes(msg);
                    socket.Send(message);
                }
            }
            else if (data.IndexOf("CONNECT") > -1) // NCC receives from a router a HELLO message with its name and listening port
            {
                string[] smsg = data.Split(' ');
                RouterConnection routerConnection = new RouterConnection();
                routerConnection.setRouterDetails(smsg[2], smsg[4]);
                routerConnection.getConnectedRouters();
                routerConnection.updateConnectedRouters();
            }
            else if (data.Contains("RESOURCE_ALLOCATION_COORDINATION")) // NCC receives from adjacent NCC a request to allocate resources for a connection
            {
                ResourceAllocationCoordination rac = new ResourceAllocationCoordination();
            }
        }
    }
}