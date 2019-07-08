using LightService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            LightService.LightService lightservice = new LightService.LightService();
            lightservice.Start();
            Console.ReadKey();
        }
    }
}
