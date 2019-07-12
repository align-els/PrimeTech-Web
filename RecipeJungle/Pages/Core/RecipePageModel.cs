using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeJungle.Entities;
using Tasky.Infrastructure;

namespace RecipeJungle.Pages
{
    [ServiceFilter(typeof(RecipePageFilter))]
    public class RecipePageModel : PageModel {
        public string ActiveNavigation { get; set; }
        public User GetCurrentUser()
        {
            return HttpContext.Items["user"] as User;
        }
    }
}
