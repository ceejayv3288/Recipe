using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface IRecipeIngredientRepository
    {
        ICollection<RecipeIngredientModel> GetRecipeIngredients();
        RecipeIngredientModel GetRecipeIngredient(int ingredientId);
        bool RecipeIngredientExists(string name, int ingredientId);
        bool RecipeIngredientExists(int id);
        bool CreateRecipeIngredient(RecipeIngredientModel recipeIngredient);
        bool UpdateRecipeIngredient(RecipeIngredientModel recipeIngredient);
        bool DeleteRecipeIngredient(RecipeIngredientModel recipeIngredient);
        bool Save();
    }
}
