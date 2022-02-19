using Recipe.Data;
using Recipe.Models;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace Recipe.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _db;

        public LikeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateLike(Like like)
        {
            _db.Likes.Add(like);
            return Save();
        }

        public bool DeleteLike(Like like)
        {
            _db.Likes.Remove(like);
            return Save();
        }

        public Like GetLike(int likeId)
        {
            return _db.Likes.FirstOrDefault(x => x.Id == likeId);
        }

        public ICollection<Like> GetLikes()
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

        public bool UpdateLike(Like like)
        {
            _db.Likes.Update(like);
            return Save();
        }
    }
}
