using RecipeAPI.Models;
using System.Collections.Generic;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface ILikeRepository
    {
        ICollection<LikeModel> GetLikes();

        ICollection<LikeModel> GetLikesByRecipeId(int recipeId);

        ICollection<LikeModel> GetLikesByUserId(string userId);

        ICollection<LikeModel> GetLikesByRecipeAndUserId(int recipeId, string userId);

        LikeModel GetLike(int likeId);

        bool LikeExists(string name);

        bool LikeExists(int id);

        bool CreateLike(LikeModel like);

        bool UpdateLike(LikeModel like);

        bool DeleteLike(LikeModel like);

        bool Save();
    }
}
