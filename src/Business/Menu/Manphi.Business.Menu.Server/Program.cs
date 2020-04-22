using System;
using Jimu.Server;

namespace Manphi.Business.Menu.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Menu Server starting ...");
            ApplicationHostServer.Instance.Run();
        }
    }
}
