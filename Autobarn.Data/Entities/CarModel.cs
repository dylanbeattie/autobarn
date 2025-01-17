﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Autobarn.Data.Entities {
	public class CarModel {
		[JsonIgnore]
		public Make Make { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		
		[JsonIgnore]
		public List<Car> Cars { get; set; } = new List<Car>();

		[JsonIgnore]
		public string Uri { get { return $"/api/carmodels/{this.Code}"; } }

		public override string ToString() => $"{Make.Name} {Name}";
	}
}

