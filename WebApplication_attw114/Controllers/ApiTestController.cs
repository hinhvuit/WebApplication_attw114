using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using System.Collections.Generic;

namespace WebApplication_attw114.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult getTest()
        {
            return StatusCode(200, "zxcvxc");
        }

        [HttpPost]
        public IEnumerable<string> TestPost()
        {
            return new string[] { "Varible1","Varible1 Content" };
        }

    }
}
