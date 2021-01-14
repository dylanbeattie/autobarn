using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Autobarn.PricingServer;
using Autobarn.Messages;
using EasyNetQ;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR.Client;

namespace Autobarn.PricingClient {
	class Program {
        private const string SIGNALR_URL = "https://workshop.ursatile.com:5001/newcarhub";
        static IBus bus;
        static Pricer.PricerClient client;
        static HubConnection hub;

		const string AMQP = "amqps://uzvpuvak:2S_t9clRGI16R8Kx6MbaYdUKc0XXGq7p@rattlesnake.rmq.cloudamqp.com/uzvpuvak";
		static async Task Main(string[] args) {
			hub = new HubConnectionBuilder().WithUrl(SIGNALR_URL).Build();
            await hub.StartAsync();
            await hub.SendAsync("NewCarAlert", "Autobarn.PricingClient", "PricingClient has connected! Yay!");
            Console.WriteLine("Sent alert message");
			using var grpc = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003");
			client = new Pricer.PricerClient(grpc);
			bus = RabbitHutch.CreateBus(AMQP);
			bus.PubSub.Subscribe<NewCarMessage>("new-car-logger", HandleNewCarMessage);
			Console.ReadKey();
		}

        static async void HandleNewCarMessage(NewCarMessage message) {
            var request = new PriceRequest {
                Make = message.Make,
                Model = message.Model,
                Year = message.Year,
                Colour = message.Color
            };
            var reply = client.GetPrice(request);
            Console.WriteLine(reply);            
            var signalRMessage = new {
                make = message.Make,
                model = message.Model,
                year = message.Year,
                color = message.Color,
                price = reply.Price,
                currency = reply.Currency
            };
            await hub.SendAsync("SendMessage", "Autobarn.PricingClient", JsonConvert.SerializeObject(signalRMessage));
        }
	}
}
