using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Wrappers
{
    public class UpdateRecipeRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<string> Tags { get; set; }
        public string[] Ingredients { get; set; }
        public List<int> Photos { get; set; }
        public int PrepareTime { get; set; }
        public int Portion { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
