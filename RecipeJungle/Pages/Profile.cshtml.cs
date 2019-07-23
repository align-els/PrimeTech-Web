using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeJungle.Contexts;
using RecipeJungle.Entities;
using RecipeJungle.Services;

namespace RecipeJungle.Pages
{
    public class ProfileModel : RecipePageModel
    {
        public RecipeContext recipeContext;

        public ProfileModel(RecipeContext recipeContext)
        {
            this.recipeContext = recipeContext;
        }

        public int GetCurrentValue() {
            var name = GetCurrentUser().Username;
            var v = recipeContext.Preferences.FirstOrDefault(x => x.Username == name);
            if (v == null) {
                recipeContext.Preferences.Add(new Preference {
                    Username = name,
                    Value = 4
                });
                recipeContext.SaveChanges();
                v = recipeContext.Preferences.FirstOrDefault(x => x.Username == name);
            }
            
            return v.Value;
        }
    }
}