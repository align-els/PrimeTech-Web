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
        public IUserService userService;
        public IRecipeService recipeService;
        public User user;
        public List<Recipe> myRecipes;
        public string searchText;

        public MyRecipesModel(IUserService userService, IRecipeService recipeService)
        {
            this.userService = userService;
            this.recipeService = recipeService;
        }
            
        public void OnGet(string search)
        {
            if (search != null)
                search = search.Trim();

            if (string.IsNullOrWhiteSpace(search))
                search = null;

            searchText = search ?? "";

            user = userService.FindByUserName("aaa");
            myRecipes = recipeService.SearchByQueryAndUser(search, GetCurrentUser());
        }
    }
}