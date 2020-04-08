using System;
using System.Threading;
using System.Threading.Tasks;
using CoreService.UDPProjectCars2.PacketParser;
using CoreService.UDPProjectCars2.RawPacketHandler;
using CoreService.UDPProjectCars2.StdDataConvertor;
using Global.Observable;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace WebUI {
    public class Program {
        public static void Main(string[] args) {
            Task.Run(() => Test());
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Test() {
            var rawHandler = PC2RawHandler.Create(5606);
            var packetParser = new PC2PacketParser(rawHandler);
            var throttlePipeline = new PC2ThrottlePositionPipeline(packetParser);
            throttlePipeline.Subscribe(new Observer<ThrottlePosition>(OnThrottle));
            rawHandler.Start();
        }

        private static void OnThrottle(ThrottlePosition throttle) {
            Startup.TestHub.Clients.All.SendAsync("ReceiveMessage", throttle.Value);
        }
    }
}
