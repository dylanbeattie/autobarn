using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Autobarn.PricingServer;

namespace Autobarn.PricingClient {
	class Program {
		static async Task Main(string[] args) {
			using var grpc = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003");
			var client = new Pricer.PricerClient(grpc);
            var random = new Random();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
			for(var i = 0; i < 1000; i++) {
				var request = new PriceRequest {
					Make = "dmc",
					Model = "delorean",
					Colour = "Silver",
					Year = 1960 + random.Next(60)
				};
				var reply = await client.GetPriceAsync(request);
			}
            sw.Stop();
            Console.WriteLine($"1000 cars took {sw.ElapsedMilliseconds}ms");
			Console.ReadKey();
		}
	}
}
