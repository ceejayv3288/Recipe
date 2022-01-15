using Recipe.Data;
using Recipe.Models;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace Recipe.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CommentExists(string description)
        {
            bool value = _db.Comments.Any(x => x.Description.ToLower().Trim() == description.ToLower().Trim());
            return value;
        }

        public bool CommentExists(int id)
        {
            return _db.Comments.Any(x => x.Id == id);
        }

        public bool CreateComment(Comment comment)
        {
            _db.Comments.Add(comment);
            return Save();
        }

        public bool DeleteComment(Comment comment)
        {
            _db.Comments.Remove(comment);
            return Save();
        }

        public Comment GetComment(int commentId)
        {
            return _db.Comments.FirstOrDefault(x => x.Id == commentId);
        }

        public ICollection<Comment> GetComments()
        {
            return _db.Comments.OrderBy(x => x.Description).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateComment(Comment comment)
        {
            _db.Comments.Update(comment);
            return Save();
        }
    }
}
