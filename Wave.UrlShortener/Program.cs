using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFX.ApplicationModel;
using NFX.Wave;
using NFX.DataAccess.Cache;

namespace Wave.UrlShortener
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var app = new ServiceBaseApplication(args, null))
                using (var server = new WaveServer())
                {
                    server.Configure(null);
                    server.Start();
                    Console.WriteLine("server started...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical error:");
                Console.WriteLine(ex);
                Environment.ExitCode = -1;
            }
        }
    }
}
