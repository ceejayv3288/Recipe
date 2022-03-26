using RecipeAPI.Models;
using System.Collections.Generic;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface IRecipeRepository
    {
        ICollection<RecipeModel> GetRecipes();

        ICollection<RecipeModel> GetPopularRecipes();

        ICollection<RecipeModel> GetPopularRecipesWithUserId(string userId);

        ICollection<RecipeModel> GetRecipesByUser(string userId);

        RecipeModel GetRecipe(int recipeId);

        bool RecipeExists(string name);

        bool RecipeExists(int id);

        bool CreateRecipe(RecipeModel recipe);

        bool UpdateRecipe(RecipeModel recipe);

        bool DeleteRecipe(RecipeModel recipe);

        bool Save();
    }
}
