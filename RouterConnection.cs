using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCC
{
    class RouterConnection
    {

        string routerName;
        string routerPort;
        Dictionary<string, string> connectedRouters = new Dictionary<string, string>();
        string path = System.IO.Directory.GetCurrentDirectory() + "/connectedRouters.txt";

        public RouterConnection(string rn, string rp)
        {
            this.routerName = rn;
            this.routerPort = rp;
        }

        public void updateConnectedRouters()
        {
            try
            {
                connectedRouters.Add(routerName, routerPort);
                System.IO.File.WriteAllText(path, String.Empty);
                string s = GetLine(connectedRouters);
                System.IO.File.WriteAllText(path, s);
            } catch
            {

            }
        }

        string GetLine(Dictionary<string, string> d)
        {
            // Build up each line one-by-one and then trim the end
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in d)
            {
                builder.Append(pair.Key).Append(" ").Append(pair.Value).Append('\n');
            }
            string result = builder.ToString();
            // Remove the final delimiter
            result = result.TrimEnd(',');
            return result;
        }

        public void getConnectedRouter()
        {
            string[] config = System.IO.File.ReadAllLines(path);
            for (int i = 0; i < config.Length; i++)
            {
                string[] entry = config[i].Split(' ');
                connectedRouters.Add(entry[0], entry[1]);
            }
            foreach (var entry in connectedRouters)
            {
                Console.WriteLine("Router: {0} Port: {1}", entry.Key, entry.Value);
            }
        }
    }
}
