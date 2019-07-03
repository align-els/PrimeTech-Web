using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace RecipeJungle.Helpers
{
    public static class NavigationMenuGenerator
    {
        public static string Generate(string activePath) {
            if (activePath != null && activePath[0] != '/')
                activePath = '/' + activePath;
            
            var items = JsonConvert.DeserializeObject<List<NavigationMenuItem>>(File.ReadAllText("navigation-menu.json"));
            var sb = new StringBuilder();
            AppendList(0, items, sb, "", activePath);
            return sb.ToString();
        }

        private static void AppendList(int level, List<NavigationMenuItem> items, StringBuilder sb, string currentPath, string activePath) {
            if (items == null || items.Count == 0)
                return;

            if (level == 0)
                sb.Append("<ul>");
            else
                sb.Append("<ul class='dropdown'>");

            foreach (var item in items) {
                string href = HttpUtility.HtmlEncode(item.HRef ?? "");
                string text = HttpUtility.HtmlEncode(item.Text ?? "");

                string newPath = currentPath + "/" + item.Text;
                if (newPath == activePath)
                    sb.Append("<li class='active'>");
                else
                    sb.Append("<li>");

                sb.Append($"<a href='{href}'>{text}</a>");
                AppendList(level + 1, item.Items, sb, newPath, activePath);
                sb.Append("</li>");
            }

            sb.Append("</ul>");
        }
    }
}
