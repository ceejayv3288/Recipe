using RecipeAPI.Models;
using System.Collections.Generic;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface ILikeRepository
    {
        ICollection<LikeModel> GetLikes();
        LikeModel GetLike(int likeId);
        bool LikeExists(string name);
        bool LikeExists(int id);
        bool CreateLike(LikeModel like);
        bool UpdateLike(LikeModel like);
        bool DeleteLike(LikeModel like);
        bool Save();
    }
}
