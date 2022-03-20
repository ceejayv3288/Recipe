using Microsoft.EntityFrameworkCore;
using RecipeAPI.Constants;
using RecipeAPI.Data;
using RecipeAPI.Models;
using RecipeAPI.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeAPI.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _db;

        public RecipeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateRecipe(RecipeModel recipe)
        {
            _db.Recipes.Add(recipe);
            return Save();
        }

        public bool DeleteRecipe(RecipeModel recipe)
        {
            _db.Recipes.Remove(recipe);
            return Save();
        }

        public ICollection<RecipeModel> GetPopularRecipes()
        {
            List<RecipeModel> popularRecipes = new List<RecipeModel>();

            foreach (int type in Enum.GetValues(typeof(CourseTypeEnum)))
            {
                if (type == (int)CourseTypeEnum.None)
                    continue;

                popularRecipes.AddRange
               (
                   _db.Recipes.Include(c => c.User).Where(d => (int)d.CourseType == type)
                   .Join(_db.Likes,
                       recipe => recipe.Id,
                       like => like.RecipeId,
                       (recipe, like) => new RecipeModel
                       {
                           CourseType = recipe.CourseType,
                           DateCreated = recipe.DateCreated,
                           DateUpdated = recipe.DateUpdated,
                           Description = recipe.Description,
                           DurationInMin = recipe.DurationInMin,
                           Id = recipe.Id,
                           Image = recipe.Image,
                           Name = recipe.Name,
                           UserId = recipe.UserId,
                           LikesCount = _db.Likes.Count(x => x.RecipeId == recipe.Id)
                       }
                   ).OrderByDescending(x => x.LikesCount)
                   .Take(5)
                   .Distinct()
                   .ToList()
               );
            }

            return popularRecipes;
        }

        public RecipeModel GetRecipe(int recipeId)
        {
            return _db.Recipes.Include(c => c.User).FirstOrDefault(x => x.Id == recipeId);
        }

        public ICollection<RecipeModel> GetRecipes()
        {
            return _db.Recipes.Include(c => c.User).OrderBy(x => x.Name).ToList();
        }

        public ICollection<RecipeModel> GetRecipesByUser(string userId)
        {
            return _db.Recipes.Include(c => c.User).Where(u => u.UserId == userId).OrderBy(x => x.Name).ToList();
        }

        public bool RecipeExists(string name)
        {
            bool value = _db.Recipes.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool RecipeExists(int id)
        {
            return _db.Recipes.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateRecipe(RecipeModel recipe)
        {
            _db.Recipes.Update(recipe);
            return Save();
        }
    }
}
