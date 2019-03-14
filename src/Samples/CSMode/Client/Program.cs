﻿using System;
using Autofac;
using Jimu;
using Jimu.Client;
using IServices;
using System.Threading;
using System.Diagnostics;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello client!");
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1NTI1ODM4NjUsInVzZXJuYW1lIjoiYWRtaW4iLCJkZXBhcnRtZW50IjoiSVTpg6gifQ.tx4etoJenyjsujHP5QGwSlhgyl9n2ftn-UziyGIIDPo";

            var container = new ContainerBuilder();
            var host = new ServiceHostClientBuilder(container)
                .UseLog4netLogger(new LogOptions { EnableConsoleLog = true })
                .UsePollingAddressSelector()
                .UseConsulForDiscovery("127.0.0.1", 8500, "JimuService-")
                .UseHttpForTransfer()
                .UseDotNettyForTransfer()
                .UseToken(() => token)
                .UseServiceProxy(new[] { "IServices" })
                .Build();
            host.Run();

            Stopwatch watch = new Stopwatch();
            while (Console.Read() != 'q')
            {
                watch.Reset();
                watch.Start();

                var proxy = host.Container.Resolve<IServiceProxy>();
                var echo = proxy.GetService<IEchoService>();
                var name = echo.GetEcho("test");
                watch.Stop();
                Console.WriteLine($"take time {watch.ElapsedMilliseconds}," + "return:  " + name);
            }

            Console.ReadKey();


        }
    }
}
