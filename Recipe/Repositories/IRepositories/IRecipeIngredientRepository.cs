using Recipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe.Repositories.IRepositories
{
    public interface IRecipeIngredientRepository
    {
        ICollection<RecipeIngredient> GetRecipeIngredients();
        RecipeIngredient GetRecipeIngredient(int ingredientId);
        bool RecipeIngredientExists(string name, int ingredientId);
        bool RecipeIngredientExists(int id);
        bool CreateRecipeIngredient(RecipeIngredient recipeIngredient);
        bool UpdateRecipeIngredient(RecipeIngredient recipeIngredient);
        bool DeleteRecipeIngredient(RecipeIngredient recipeIngredient);
        bool Save();
    }
}
