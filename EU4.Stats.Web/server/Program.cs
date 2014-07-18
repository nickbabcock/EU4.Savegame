using Nancy.Hosting.Self;
using System;
using System.Linq;
using System.Threading;

namespace EU4.Stats.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = "http://localhost:8888";
            Console.WriteLine(uri);
            var host = new NancyHost(new Uri(uri));
            host.Start();

            // Under mono if you deamonize a process a Console.ReadLine with cause an EOF 
            // so we need to block another way
            if (args.Any(s => s.Equals("-d", StringComparison.CurrentCultureIgnoreCase)))
            {
                Thread.Sleep(Timeout.Infinite);
            }
            else
            {
                Console.ReadKey();
            }

            host.Stop();  // stop hosting
        }
    }
}
