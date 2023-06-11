using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQP_EyeTestRelay
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting UDP Listener...");

            RelayUDP relay = new RelayUDP(10024, "COM5");

            Console.Title = "Meta Quest Pro - Eye Display Test Relay App (Press Enter to Quit)";

            Console.ReadLine();

            Console.WriteLine("Closing");

            relay.Shutdown();
        }
    }
}
