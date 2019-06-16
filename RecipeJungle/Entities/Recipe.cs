using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Entities {
    public class Recipe {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Text { get; set; }
        public List<RecipeTag> RecipeTags { get; set; }
        public List<UserRecipe> RecipeLikes { get; set; }
        public string Ingredients { get; set; }
        public List<Photo> Photos { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int PrepareTime { get; set; }
        public int Portion { get; set; }
        public User User { get; set; }
    }
}
