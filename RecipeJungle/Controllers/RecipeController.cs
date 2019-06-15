using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeJungle.Entities;
using RecipeJungle.Filters;
using RecipeJungle.Services;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Controllers
{
    [Authorize]
    [UserFilter]
    [Route("/api/recipe")]
    public class RecipeController : ControllerBase {
        private IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService) {
            this.recipeService = recipeService;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateRecipeRequest request,[FromHeader] User user) {
            recipeService.CreateRecipe(request,user);
            return ActionUtils.Success();
        }

        [HttpGet("list")]
        public IActionResult List() {
            return ActionUtils.Success(recipeService.ListRecipes());
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody] UpdateRecipeRequest request,[FromHeader] User user) {
            recipeService.UpdateRecipes(request);
            return ActionUtils.Success();
        }
        
        [HttpDelete("delete")]
        public IActionResult Delete(int id, User user)
        {
            recipeService.DeleteRecipe(id,user);
            return ActionUtils.Success();
        }

        [HttpGet("listWithLabels")] //bunun yeri burası mı ki 
        public IActionResult ListWithLabels(int id)
        {
            return ActionUtils.Success(recipeService.ListWithLabels(id));
        }
    }
}
