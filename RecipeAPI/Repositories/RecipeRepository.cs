using Microsoft.EntityFrameworkCore;
using RecipeAPI.Data;
using RecipeAPI.Models;
using RecipeAPI.Repositories.IRepositories;
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

        public RecipeModel GetRecipe(int recipeId)
        {
            return _db.Recipes.Include(c => c.User).FirstOrDefault(x => x.Id == recipeId);
        }

        public ICollection<RecipeModel> GetRecipes()
        {
            return _db.Recipes.Include(c => c.User).OrderBy(x => x.Name).ToList();
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
