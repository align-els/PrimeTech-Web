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
    public class HomeModel : RecipePageModel
    {
        public Services.IRecipeService _recipeService;
        public Recipe recipe;
        public List<Recipe> recipes;

        public HomeModel(Services.IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public void OnGet()
        {
            recipes = _recipeService.ListRecipes();
        }
    }
}