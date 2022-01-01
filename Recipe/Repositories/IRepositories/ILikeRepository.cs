using Recipe.Models;
using System.Collections.Generic;

namespace Recipe.Repositories.IRepositories
{
    public interface ILikeRepository
    {
        ICollection<Like> GetLikes();
        Like GetLike(int likeId);
        bool LikeExists(string name);
        bool LikeExists(int id);
        bool CreateLike(Like like);
        bool UpdateLike(Like like);
        bool DeleteLike(Like like);
        bool Save();
    }
}
