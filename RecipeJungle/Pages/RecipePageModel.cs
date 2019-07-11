using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeJungle.Pages
{
    //[ServiceFilter(typeof(...))]
    public class RecipePageModel : PageModel {
        public string ActiveNavigation { get; set; }
    }
}
