using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeJungle.Entities;

namespace RecipeJungle.Wrappers
{
    public class CreateRecipeRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string[] Tags { get; set; }
        public string[] Ingredients { get; set; }
        public string[] Steps { get; set; }
        public List<string> Photos { get; set; }
        public int PrepareTime { get; set; }
        public int Portion { get; set; }
    }
}
