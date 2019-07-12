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
        public IUserService _userService;
        public User user;
        public List<Recipe> myRecipes;

        public MyRecipesModel(IUserService userService)
        {
            _userService = userService;
        }
            
        public void OnGet()
        {
            user= _userService.FindByUserName("aaa");
            myRecipes= _userService.ListMyRecipes(user);
        }
    }
}