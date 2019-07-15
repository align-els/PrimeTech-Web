using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeJungle.Entities;
using RecipeJungle.Services;

namespace RecipeJungle.Pages
{
    public class RecipeDetailsModel : RecipePageModel
    {
        public IRecipeService recipeService;
        public Recipe Recipe;
        public bool IsOwnPost;
        public int LikeCount;
        public bool UserLiked;

        public RecipeDetailsModel(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public void OnGet(int id)
        {
            Recipe = recipeService.GetReceiveById(id);
            IsOwnPost = Recipe.User.Id == GetCurrentUser().Id;

            UserLiked = recipeService.IsUserLiked(id, GetCurrentUser().Id);
            LikeCount = recipeService.GetLikeCount(id);
        }
    }
}
