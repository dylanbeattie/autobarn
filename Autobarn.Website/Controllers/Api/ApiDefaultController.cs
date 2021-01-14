using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.Api {
	[Route("api/")]
	[ApiController]
	public class ApiDefaultController : ControllerBase {
		[HttpGet]
		public ActionResult Get() {
			var json = new {
				message = "Welcome to the Autobarn API",
				_links = new {
					cars = new {
						name = "Get all cars",
						href = "/api/cars"
					},
					car = new {
						name = "Find a car",
						href = "/api/cars/{registration}"
					}
				},
				_actions = new {
					add = new {
						name = "Add a car to the Autobarn database",
						href = "/api/cars",
						method = "POST",
						schema = "/schemas/newcar",
						type = "application/json"
					}
				}
			};
			return (Ok(json));
		}
	}
}
