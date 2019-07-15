using System.Collections.Generic;
using RecipeJungle.Entities;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Services {
    public interface IRecipeService {
        void CreateRecipe(CreateRecipeRequest request, User user);
        void DeleteRecipe(int id, User user);
        List<Recipe> ListWithLabels(int id);
        List<Recipe> ListRecipes();
        void UpdateRecipes(UpdateRecipeRequest request);
        void LikeRecipe(int id, User user);
        List<Recipe> GlobalSearch(string query);
        Recipe GetReceiveById(int id);
        string[] GetTagsOfRecipe(Recipe recipe);
        List<Recipe> ListMyRecipes(User user);
        List<Recipe> SearchByQueryAndUser(string search, User user);
    }
}