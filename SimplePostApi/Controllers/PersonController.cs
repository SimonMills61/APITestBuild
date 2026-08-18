using Microsoft.AspNetCore.Mvc;

namespace SimplePostApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        public class Person
        {
            public string? Name { get; set; }
            public int Age { get; set; }
        }

        [HttpPost]
        public IActionResult CreatePerson([FromBody] Person person)
        {
            return Ok(new {
                Message = "Person received",
                Data = person
            });
        }
    }
}
