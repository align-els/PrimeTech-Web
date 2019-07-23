using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeJungle.Entities
{
    public class Preference
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Value { get; set; }
    }
}
