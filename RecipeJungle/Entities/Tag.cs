using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Entities {

    public class Tag {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
