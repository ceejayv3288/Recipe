using Microsoft.EntityFrameworkCore;
using RecipeAPI.Data;
using RecipeAPI.Models;
using RecipeAPI.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Repositories
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly ApplicationDbContext _db;

        public RecipeIngredientRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateRecipeIngredient(RecipeIngredientModel recipeIngredient)
        {
            _db.RecipeIngredients.Add(recipeIngredient);
            return Save();
        }

        public bool DeleteRecipeIngredient(RecipeIngredientModel recipeIngredient)
        {
            _db.RecipeIngredients.Remove(recipeIngredient);
            return Save();
        }

        public ICollection<RecipeIngredientModel> GetRecipeIngredients()
        {
            return _db.RecipeIngredients.Include(c => c.Recipe).OrderBy(x => x.Recipe.Name).ToList();
        }

        public RecipeIngredientModel GetRecipeIngredient(int ingredientId)
        {
            return _db.RecipeIngredients.Include(c => c.Recipe).FirstOrDefault(x => x.Id == ingredientId);
        }

        public bool RecipeIngredientExists(string name, int ingredientId)
        {
            bool value = _db.RecipeIngredients.Any(x => x.Recipe.Name.ToLower().Trim() == name.ToLower().Trim() && x.Id == ingredientId);
            return value;
        }

        public bool RecipeIngredientExists(int id)
        {
            return _db.RecipeIngredients.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateRecipeIngredient(RecipeIngredientModel recipeIngredient)
        {
            _db.RecipeIngredients.Update(recipeIngredient);
            return Save();
        }

        public ICollection<RecipeIngredientModel> GetRecipeIngredientsByRecipeId(int recipeId)
        {
            return _db.RecipeIngredients.Include(c => c.Recipe).Where(r => r.Recipe.Id == recipeId).OrderBy(x => x.Recipe.Name).ToList();
        }
    }
}
