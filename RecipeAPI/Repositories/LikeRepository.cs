using RecipeAPI.Data;
using RecipeAPI.Models;
using RecipeAPI.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace RecipeAPI.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _db;

        public LikeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateLike(LikeModel like)
        {
            _db.Likes.Add(like);
            return Save();
        }

        public bool DeleteLike(LikeModel like)
        {
            _db.Likes.Remove(like);
            return Save();
        }

        public LikeModel GetLike(int likeId)
        {
            return _db.Likes.FirstOrDefault(x => x.Id == likeId);
        }

        public ICollection<LikeModel> GetLikes()
        {
            return _db.Likes.OrderBy(x => x.Recipe.Name).ToList();
        }

        public bool LikeExists(string name)
        {
            bool value = _db.Likes.Any(x => x.Recipe.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool LikeExists(int id)
        {
            return _db.Likes.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateLike(LikeModel like)
        {
            _db.Likes.Update(like);
            return Save();
        }
    }
}
