using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeJungle.Entities;

namespace RecipeJungle.Contexts
{
    public class TestContext : DbContext {
        public DbSet<TestEntity> TestEntities { get; set; }

        public TestContext(DbContextOptions<TestContext> options)
            : base(options) {
        }
    }
}
