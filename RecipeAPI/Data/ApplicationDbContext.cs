using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;

namespace RecipeAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<IdentityUser>().ToTable("Users").Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<UserModel>().ToTable("Users").Property(p => p.Id).HasColumnName("Id");
        }

        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<RecipeStepModel> RecipeSteps { get; set; }
        public DbSet<RecipeIngredientModel> RecipeIngredients { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
