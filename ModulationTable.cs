using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace NCC
{
    class ModulationTable
    {

        public ModulationEntry[] mt = new ModulationEntry[6];
        
        public ModulationTable() {
            init();
        }

        public void init() {
            string path = System.IO.Directory.GetCurrentDirectory() + "/modulations.txt";
            string[] config = System.IO.File.ReadAllLines(path);
            for(int i=0; i<config.Length; i++) {
                string[] entry = config[i].Split(' ');
                mt[i] = new ModulationEntry(Int32.Parse(entry[0]), Int32.Parse(entry[1]), entry[2], Int32.Parse(entry[3]));
            }

        }

        public string getModulation(int distance) {
            foreach(var m in mt) {
                if(m.getMinDist() < distance && m.getMaxDist() > distance) 
                    return m.getModulation();
            }
            return "BPSK";
        }

        public int getModulationMultiplier(string modulation) {
            foreach(var m in mt)
                if(m.getModulation() == modulation)
                    return m.getMultiplier();
            return 1;
        }

        public void showModulationTable() {
            Console.WriteLine("Printing Modulation Table");
            foreach(var m in mt) 
                Console.WriteLine("MIN DISTANCE: {0} MAX DISTANCE: {1} MODULATION: {2} MULTIPLIER: {3}", m.getMinDist(), m.getMaxDist(), m.getModulation(), m.getMultiplier());
        }
    }
}
