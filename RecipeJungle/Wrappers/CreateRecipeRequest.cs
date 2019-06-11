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
        public List<Tag> Tags { get; set; }
        public string Ingredients { get; set; }
        public List<Photo> Photos { get; set; }
        public int PrepareTime { get; set; }
        public int Portion { get; set; }
    }
}
