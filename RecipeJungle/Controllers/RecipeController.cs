using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecipeJungle.Entities;
using RecipeJungle.Exceptions;
using RecipeJungle.Services;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Controllers
{
    [Route("/api/recipe")]
    public class RecipeController : ControllerBase {
        private IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService) {
            this.recipeService = recipeService;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateRecipeRequest request) {
            recipeService.CreateRecipe(request);
            return ActionUtils.Success();
        }

        [HttpGet("list")]
        public IActionResult List() {
            return ActionUtils.Success(recipeService.ListRecipes());
        }
    }
}
