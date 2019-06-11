using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeJungle.Entities;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Services
{
    public interface IRecipeService
    {
        void CreateRecipe(CreateRecipeRequest request);
        List<Recipe> ListRecipes();
    }
}
