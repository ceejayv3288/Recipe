using Recipe.Models;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;

namespace Recipe.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        public bool CreateRecipe(RecipeModel recipe)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteRecipe(RecipeModel recipe)
        {
            throw new System.NotImplementedException();
        }

        public RecipeModel GetRecipe(int recipeId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<RecipeModel> GetRecipes()
        {
            throw new System.NotImplementedException();
        }

        public bool RecipeExists(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool RecipeExists(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Save()
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateRecipe(RecipeModel recipe)
        {
            throw new System.NotImplementedException();
        }
    }
}
