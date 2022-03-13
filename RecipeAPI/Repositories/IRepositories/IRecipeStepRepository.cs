using RecipeAPI.Models;
using System.Collections.Generic;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface IRecipeStepRepository
    {
        ICollection<RecipeStepModel> GetRecipeSteps();

        ICollection<RecipeStepModel> GetRecipeStepsByRecipeId(int recipeId);

        RecipeStepModel GetRecipeStep(int recipeStepId);

        bool RecipeStepExists(string name, int stepId);

        bool RecipeStepExists(int id);

        bool CreateRecipeStep(RecipeStepModel recipeStep);

        bool UpdateRecipeStep(RecipeStepModel recipeStep);

        bool DeleteRecipeStep(RecipeStepModel recipeStep);

        bool Save();
    }
}
