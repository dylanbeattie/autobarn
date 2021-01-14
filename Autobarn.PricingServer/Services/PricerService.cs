using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer {
	public class PricerService : Pricer.PricerBase {
		private readonly ILogger<PricerService> _logger;
		public PricerService(ILogger<PricerService> logger) {
			_logger = logger;
		}

		public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
			(int price, string currency) = CalculatePrice(request.Make, request.Model, request.Year, request.Colour);
			return Task.FromResult(new PriceReply {
				Price = price, Currency = currency
			});
		}

		private (int, string) CalculatePrice(string make, string model, int year, string color) {
			if (color.Contains("blue")) return (50, "EUR");
			if (year < 2000) return (1000, "GBP");
			return (25000, "USD");
		}
	}
}
