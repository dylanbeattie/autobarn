using System;
using EasyNetQ;
using Autobarn.Messages;

namespace Autobarn.NewCarLogger {
	class Program {
		const string AMQP = "amqps://uzvpuvak:2S_t9clRGI16R8Kx6MbaYdUKc0XXGq7p@rattlesnake.rmq.cloudamqp.com/uzvpuvak";
		static void Main(string[] args) {
			using var bus = RabbitHutch.CreateBus(AMQP);
            bus.PubSub.Subscribe<NewCarMessage>("new-car-logger", HandleNewCarMessage);
            Console.ReadKey();
		}

        static void HandleNewCarMessage(NewCarMessage message) {
            Console.WriteLine($"Handling new car for {message.Registration}");
            var record = $"{message.Registration},{message.Make},{message.Model},{message.Year},{message.Color}";
            System.IO.File.AppendAllText("new_cars.log", record + Environment.NewLine);
        }
	}
}
