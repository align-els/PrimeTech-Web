using System.Collections.Generic;
using RecipeJungle.Entities;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Services
{
    public interface IRecipeService
    {
        void CreateRecipe(CreateRecipeRequest request, User user);
        void DeleteRecipe(int id, User user);
        List<Recipe> ListWithLabels(int id);
        List<Recipe> ListRecipes();
        void UpdateRecipes(UpdateRecipeRequest request);
        void LikeRecipe(int id, User user);
    }
}
