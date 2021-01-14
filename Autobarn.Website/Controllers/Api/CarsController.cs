using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using System.Linq;

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
		public ActionResult Get(string id) {
			var car = db.FindCar(id);
			if (car == default) return NotFound("Sorry - we couldn't find that car in our database!");
			var json = new {
				links = new {
					carModel = new {
						href = car.CarModel.Uri
					}
				},
				item = car
			};
			return Ok(json);
		}

		// POST api/<CarsController>
		[HttpPost]
		public ActionResult Post([FromBody] CarPostModel post) {
			var existingCar = db.FindCar(post.Registration);
			if (existingCar != default) return Conflict($"Sorry - the car with registration {post.Registration} is already listed!");
			var model =  db.Models.FirstOrDefault(m  => m.Uri == post.Model);
			if (model == default) return BadRequest("We don't recognise that car model - sorry!");			
			var car = new Autobarn.Data.Entities.Car {
				Registration = post.Registration,
				Color = post.Color,
				Year = post.Year,
				CarModel = model
			};
			db.AddCar(car);
			return(Created($"/api/cars/{car.Registration}", car));
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

	public class CarPostModel {
		public string Registration { get;set; }
		public string Color {get;set;}
		public int Year { get;set; }
		public string Model {get;set;}
	}
}
