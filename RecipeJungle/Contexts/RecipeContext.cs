using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

        protected override void OnModelCreating(ModelBuilder mb) {
            mb.Entity<RecipeTag>().HasKey(x => new { x.RecipeId, x.TagId });

            mb.Entity<Tag>().HasIndex(x => x.Text).IsUnique();

            mb.Entity<UserRecipe>().HasOne(x => x.User).WithMany(x => x.RecipesOfUser);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.ConfigureWarnings(x => x.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }
    }
}
