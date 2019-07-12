using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeJungle.Contexts;
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
        public IActionResult Delete(int id, [FromHeader] User user)
        {
            recipeService.DeleteRecipe(id,user);
            return ActionUtils.Success("Delete success");
        }

        [HttpGet("listWithLabels")] 
        public IActionResult ListWithLabels(int id)
        {
            return ActionUtils.Success(recipeService.ListWithLabels(id));
        }
        [HttpPost("like")]
        public IActionResult Like(int id, [FromHeader] User user)
        {
            recipeService.LikeRecipe(id, user);
            return ActionUtils.Success();
        }
        [HttpGet("search")] 
        public IActionResult Search(string query)
        {
            return ActionUtils.Success(recipeService.GlobalSearch(query));
        }
        [HttpGet("get")]
        public IActionResult GetRecipe(int id)
        {
            return ActionUtils.Success(recipeService.GetReceiveById(id));
        }
    }
}
