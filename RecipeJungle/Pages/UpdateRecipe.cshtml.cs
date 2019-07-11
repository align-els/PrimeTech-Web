using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RecipeJungle.Pages
{
    public class UpdateRecipeModel : RecipePageModel
    {
        public RecipeJungle.Services.IRecipeService _recipeService;
        public RecipeJungle.Entities.Recipe Recipe;

        public UpdateRecipeModel(RecipeJungle.Services.IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public void OnGet(int Id)
        {
            Recipe = _recipeService.GetReceiveById(Id);
        }
    }
}