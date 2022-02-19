using Recipe.Data;
using Recipe.Models;
using Recipe.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe.Repositories
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly ApplicationDbContext _db;

        public RecipeIngredientRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            _db.RecipeIngredients.Add(recipeIngredient);
            return Save();
        }

        public bool DeleteRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            _db.RecipeIngredients.Remove(recipeIngredient);
            return Save();
        }

        public ICollection<RecipeIngredient> GetRecipeIngredients()
        {
            return _db.RecipeIngredients.OrderBy(x => x.Recipe.Name).ToList();
        }

        public RecipeIngredient GetRecipeIngredient(int ingredientId)
        {
            return _db.RecipeIngredients.FirstOrDefault(x => x.Id == ingredientId);
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

        public bool UpdateRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            _db.RecipeIngredients.Update(recipeIngredient);
            return Save();
        }
    }
}
