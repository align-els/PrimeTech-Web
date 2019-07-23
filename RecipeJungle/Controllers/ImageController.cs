using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeJungle.Filters;
using RecipeJungle.Services;

namespace RecipeJungle.Controllers
{
    [UserFilter]
    [Route("/api/photo")]
    public class PhotoController : ControllerBase {
        private IRecipeService recipeService;

        public PhotoController(IRecipeService recipeService) {
            this.recipeService = recipeService;
        }

        [HttpGet("get")]
        public IActionResult GetPhoto(int recipeId) {
            try {
                var recipe = recipeService.GetReceiveById(recipeId);
                var photos = JsonConvert.DeserializeObject<string[]>(recipe.Photos);
                var bytes = Convert.FromBase64String(photos[0]);
                return File(bytes, "image/jpeg");
            } catch {
                return Redirect("/photo_not_available.gif");
            }
        }

        [HttpGet("get-at")]
        public IActionResult GetPhotoAt(int recipeId, int index) {
            var recipe = recipeService.GetReceiveById(recipeId);
            var photos = JsonConvert.DeserializeObject<string[]>(recipe.Photos);
            var bytes = Convert.FromBase64String(photos[index]);
            return File(bytes, "image/jpeg");
        }
    }
}
