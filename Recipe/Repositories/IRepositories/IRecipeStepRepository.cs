using Recipe.Models;
using System.Collections.Generic;

namespace Recipe.Repositories.IRepositories
{
    public interface IRecipeStepRepository
    {
        ICollection<RecipeStep> GetRecipeSteps();
        RecipeStep GetRecipeStep(int recipeStepId);
        bool RecipeStepExists(string name, int stepId);
        bool RecipeStepExists(int id);
        bool CreateRecipeStep(RecipeStep recipeStep);
        bool UpdateRecipeStep(RecipeStep recipeStep);
        bool DeleteRecipeStep(RecipeStep recipeStep);
        bool Save();
    }
}
