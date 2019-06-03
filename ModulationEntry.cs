using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace NCC
{
    class ModulationEntry
    {
        public int minDist;
        public int maxDist;
        public string modulation;
        public int multiplier;

        public ModulationEntry(int minDist, int maxDist, string modulation, int multiplier)
        {
            this.minDist = minDist;
            this.maxDist = maxDist;
            this.modulation = modulation;
            this.multiplier = multiplier;
        }

        public int getMinDist()
        {
            return this.minDist;
        }

        public int getMaxDist()
        {
            return this.maxDist;
        }

        public string getModulation()
        {
            return this.modulation;
        }

        public int getMultiplier()
        {
            return this.multiplier;
        }
    }
}
