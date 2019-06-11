using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeJungle.Contexts;
using RecipeJungle.Entities;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Services
{
    public class RecipeService : IRecipeService {
        private RecipeContext recipeContext;

        public RecipeService(RecipeContext recipeContext) {
            this.recipeContext = recipeContext;
        }

        public void CreateRecipe(CreateRecipeRequest request) {
            Recipe recipe = new Recipe();
            recipe.Title = request.Title;
            recipe.Text = request.Text;
            recipe.Ingredients = request.Ingredients;
            recipe.CreatedTime = DateTime.Now;
            recipe.Id = 0;
            recipe.ModifiedTime = DateTime.Now;

            recipeContext.Recipes.Add(recipe);
            recipeContext.SaveChanges();
        }

        public List<Recipe> ListRecipes() {
            return recipeContext.Recipes.ToList();
        }
    }
}
