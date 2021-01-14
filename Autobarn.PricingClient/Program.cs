using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Autobarn.PricingServer;

namespace Autobarn.PricingClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var grpc = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003");
            var client = new Greeter.GreeterClient(grpc);
            var request = new HelloRequest { Name = "Everybody!" };
            var reply = await client.SayHelloAsync(request);
            Console.WriteLine(reply);
            Console.ReadKey();
        }
    }
}
