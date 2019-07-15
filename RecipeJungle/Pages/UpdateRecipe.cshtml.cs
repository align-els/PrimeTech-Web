using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeJungle.Entities;
using RecipeJungle.Services;

namespace RecipeJungle.Pages
{
    public class UpdateRecipeModel : RecipePageModel
    {
        public IRecipeService recipeService;
        public Recipe Recipe;

        public UpdateRecipeModel(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public void OnGet(int Id)
        {
            Recipe = recipeService.GetReceiveById(Id);
        }

        public string[] GetTags() {
            return recipeService.GetTagsOfRecipe(Recipe);
        }
    }
}