using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Entities{
    public class User{
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<Recipe> RecipesOfUser { get; set; }
        public List<Recipe> LikedRecipesOfUser { get; set; }
    }
}
