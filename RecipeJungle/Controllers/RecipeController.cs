using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeJungle.Entities;
using RecipeJungle.Services;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Controllers
{
    [Route("/api/recipe")]
    public class RecipeController : ControllerBase
    {
        private IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService) {
            this.recipeService = recipeService;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateRecipeRequest request) {
            if (request.Text == null)
                return BadRequest("text'i yaz");

            recipeService.CreateRecipe(request);
            return Ok("success");
        }

        [HttpGet("list")]
        public IActionResult List() {
            List<Recipe> res = recipeService.ListRecipes();

            return Ok(res);
        }
    }
}
