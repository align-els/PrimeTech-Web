using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Helpers
{
    public class NavigationMenuItem
    {
        public string Text { get; set; }
        public string HRef { get; set; }
        public List<NavigationMenuItem> Items { get; set; }
    }
}
