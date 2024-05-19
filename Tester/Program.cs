using InterLoinkClass.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                RequestProcessor server = new RequestProcessor();

                while (true)
                {
                    server.ProcessTraffic();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
