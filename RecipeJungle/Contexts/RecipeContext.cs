using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeJungle.Entities;

namespace RecipeJungle.Contexts
{
    public class RecipeContext : DbContext {

        public RecipeContext(DbContextOptions<RecipeContext> options)
            : base(options) {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
