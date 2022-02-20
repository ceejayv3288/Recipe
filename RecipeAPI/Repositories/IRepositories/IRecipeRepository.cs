using RecipeAPI.Models;
using System.Collections.Generic;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface IRecipeRepository
    {
        ICollection<RecipeModel> GetRecipes();
        RecipeModel GetRecipe(int recipeId);
        bool RecipeExists(string name);
        bool RecipeExists(int id);
        bool CreateRecipe(RecipeModel recipe);
        bool UpdateRecipe(RecipeModel recipe);
        bool DeleteRecipe(RecipeModel recipe);
        bool Save();
    }
}
