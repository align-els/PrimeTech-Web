using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Entities{
    public class User{
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
<<<<<<< HEAD
        public List<UserRecipe> RecipesOfUser { get; set; }
        public List<UserRecipe> LikedRecipesOfUser { get; set; }
=======
        public string Token { get; set; }
        public List<Recipe> RecipesOfUser { get; set; }
        public List<Recipe> LikedRecipesOfUser { get; set; }
        public byte[] PasswordHash { get; internal set; }
        public byte[] PasswordSalt { get; internal set; }
>>>>>>> 12108595a03abb4d400bf0a2100a5d142c382e10
    }
}
