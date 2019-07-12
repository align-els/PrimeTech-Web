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
    public class MyRecipesModel : RecipePageModel
    {
        public IRecipeService _recipeService;
        public Recipe Recipe;
        public List<Recipe> recipes;

        public MyRecipesModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }
            
        public void OnGet()
        {
             recipes = _recipeService.ListRecipes();
        }
    }
}