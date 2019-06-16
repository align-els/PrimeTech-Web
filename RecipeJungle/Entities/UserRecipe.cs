using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecipeJungle.Entities
{
    public class UserRecipe
    {
        [JsonIgnore]
        public int RecipeId { get; set; }

        [JsonIgnore]
        public Recipe Recipe { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
