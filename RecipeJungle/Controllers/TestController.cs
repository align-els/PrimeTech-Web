using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeJungle.Services;

namespace RecipeJungle.Controllers
{
    [Route("/api/test")]
    public class TestController : ControllerBase {
        private readonly IRecipeService testService;

        public TestController(IRecipeService testService) {
            this.testService = testService;
        }

        [HttpGet("hello")]
        public IActionResult Hello(string name) {
            return Ok("hello " + name + "!");
        }
    }
}
