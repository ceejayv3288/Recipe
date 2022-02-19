using Recipe.Data;
using Recipe.Models;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace Recipe.Repositories
{
    public class RecipeStepRepository : IRecipeStepRepository
    {
        private readonly ApplicationDbContext _db;

        public RecipeStepRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateRecipeStep(RecipeStep recipeStep)
        {
            _db.RecipeSteps.Add(recipeStep);
            return Save();
        }

        public bool DeleteRecipeStep(RecipeStep recipeStep)
        {
            _db.RecipeSteps.Remove(recipeStep);
            return Save();
        }

        public RecipeStep GetRecipeStep(int recipeStepId)
        {
            return _db.RecipeSteps.FirstOrDefault(x => x.Id == recipeStepId);
        }

        public ICollection<RecipeStep> GetRecipeSteps()
        {
            return _db.RecipeSteps.OrderBy(x => x.Recipe.Name).ToList();
        }

        public bool RecipeStepExists(string name, int stepId)
        {
            bool value = _db.RecipeSteps.Any(x => x.Recipe.Name.ToLower().Trim() == name.ToLower().Trim() && x.Id == stepId);
            return value;
        }

        public bool RecipeStepExists(int id)
        {
            return _db.RecipeSteps.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateRecipeStep(RecipeStep recipeStep)
        {
            _db.RecipeSteps.Update(recipeStep);
            return Save();
        }
    }
}
