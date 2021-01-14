using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autobarn.Data;
using Autobarn.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.Api {
	[Route("api/[controller]")]
	[ApiController]
	public class CarsController : ControllerBase {
		private readonly ICarDatabase db;

		public CarsController(ICarDatabase db) {
			this.db = db;
		}
		// GET: api/<CarsController>
		[HttpGet]
		public IEnumerable<Car> Get() {
			return db.Cars;
		}

		// GET api/<CarsController>/5
		[HttpGet("{id}")]
		public string Get(int id) {
			return "value";
		}

		// POST api/<CarsController>
		[HttpPost]
		public void Post([FromBody] string value) {
		}

		// PUT api/<CarsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value) {
		}

		// DELETE api/<CarsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id) {
		}
	}
}
