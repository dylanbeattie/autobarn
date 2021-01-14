using System;

namespace Autobarn.Messages {
	public class NewCarMessage {
		public string Registration { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int Year { get; set; }
		public string Color { get; set; }
	}
}
