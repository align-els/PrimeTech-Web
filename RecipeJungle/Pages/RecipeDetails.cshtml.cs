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
        public IRecipeService _recipeService;
        public Recipe Recipe;
        public bool IsOwnPost;
       
        public RecipeDetailsModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public void OnGet(int Id)
        {
            Recipe = _recipeService.GetReceiveById(Id);
            IsOwnPost = Recipe.User.Id == GetCurrentUser().Id;
        }
    }
}
