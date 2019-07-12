using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeJungle.Pages
{
    public class IndexModel : RecipePageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("/login");
        }
    }
}