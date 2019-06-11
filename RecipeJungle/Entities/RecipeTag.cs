using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecipeJungle.Entities {
    public class RecipeTag {

        [JsonIgnore]
        public int RecipeId { get; set; }

        [JsonIgnore]
        public Recipe Recipe { get; set; }

        [JsonIgnore]
        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
