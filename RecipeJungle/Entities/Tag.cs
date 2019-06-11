using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecipeJungle.Entities {

    public class Tag {
        public int Id { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        public List<RecipeTag> RecipeTags { get; set; }
    }
}
