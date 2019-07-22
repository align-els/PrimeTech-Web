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
    public class ProfileModel : RecipePageModel
    {
        public IUserService userService;

        public ProfileModel(IUserService userService)
        {
            this.userService = userService;
        }
    }
}