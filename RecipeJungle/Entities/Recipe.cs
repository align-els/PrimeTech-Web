using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Entities
{
    public class Recipe
    {
          public string Title { get; set; }
          public int Id { get; set; }
          public string Text { get; set; }
          public List<string> Tags { get; set; }
    }
}
