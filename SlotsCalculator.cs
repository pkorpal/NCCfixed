using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCC
{
    class SlotsCalculator
    {
        private const double CONVERTER = 2; //Hz/Baud
        private const double SLOT_SIZE = 12.5; //Szerokość szceliny
        private const double ADDITIONAL_FREQUENCY = 2; //Dodatkowa częstotliwość po obu stronach szczelin


        private static double frequency;
        private static int slots = 0;

        public int calculateSlots(int parameter, double throughput)
        {
            frequency = CONVERTER * throughput / parameter + ADDITIONAL_FREQUENCY;
            slots = (int)Math.Ceiling(frequency / SLOT_SIZE);

            if (slots <= 20)
            {
                Console.WriteLine("There will be " + slots + " slots needed to accomplish this connection.");
                return slots;
            }
            else
            {
                Console.WriteLine("Throughput is too large. We are not able to accomplish this connection.");
                return slots;
            }
        }
    }
}
 