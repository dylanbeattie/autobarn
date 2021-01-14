using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Autobarn.PricingServer;
using Autobarn.Messages;
using EasyNetQ;

namespace Autobarn.PricingClient {
	class Program {
        static IBus bus;
        static Pricer.PricerClient client;

		const string AMQP = "amqps://uzvpuvak:2S_t9clRGI16R8Kx6MbaYdUKc0XXGq7p@rattlesnake.rmq.cloudamqp.com/uzvpuvak";
		static void Main(string[] args) {
			using var grpc = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003");
			client = new Pricer.PricerClient(grpc);
			bus = RabbitHutch.CreateBus(AMQP);
			bus.PubSub.Subscribe<NewCarMessage>("new-car-logger", HandleNewCarMessage);
			Console.ReadKey();
		}

        static void HandleNewCarMessage(NewCarMessage message) {
            var request = new PriceRequest {
                Make = message.Make,
                Model = message.Model,
                Year = message.Year,
                Colour = message.Color
            };
            var reply = client.GetPrice(request);
            Console.WriteLine(reply);
        }




		// static async Task Main(string[] args) {
		// 	var random = new Random();
		// 	var sw = new System.Diagnostics.Stopwatch();
		// 	sw.Start();
		// 	for (var i = 0; i < 1000; i++) {
		// 		var request = new PriceRequest {
		// 			Make = "dmc",
		// 			Model = "delorean",
		// 			Colour = "Silver",
		// 			Year = 1960 + random.Next(60)
		// 		};
		// 		var reply = await client.GetPriceAsync(request);
		// 	}
		// 	sw.Stop();
		// 	Console.WriteLine($"1000 cars took {sw.ElapsedMilliseconds}ms");
		// 	Console.ReadKey();
		// }
	}
}
