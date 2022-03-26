using Microsoft.EntityFrameworkCore;
using RecipeAPI.Data;
using RecipeAPI.Models;
using RecipeAPI.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace RecipeAPI.Repositories
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

        public bool CreateComment(CommentModel comment)
        {
            _db.Comments.Add(comment);
            return Save();
        }

        public bool DeleteComment(CommentModel comment)
        {
            _db.Comments.Remove(comment);
            return Save();
        }

        public CommentModel GetComment(int commentId)
        {
            return _db.Comments.Include(c => c.Recipe).Include(d => d.User).FirstOrDefault(x => x.Id == commentId);
        }

        public ICollection<CommentModel> GetComments()
        {
            return _db.Comments.Include(c => c.Recipe).Include(d => d.User).OrderBy(x => x.Description).ToList();
        }

        public ICollection<CommentModel> GetCommentsByRecipeAndUserId(int recipeId, string userId)
        {
            return _db.Comments.Include(c => c.Recipe).Include(c => c.User).Where(r => r.Recipe.Id == recipeId && r.User.Id == userId).OrderBy(x => x.Recipe.Name).ToList();
        }

        public ICollection<CommentModel> GetCommentsByRecipeId(int recipeId)
        {
            return _db.Comments.Include(c => c.Recipe).Where(r => r.Recipe.Id == recipeId).OrderBy(x => x.Recipe.Name).ToList();
        }

        public ICollection<CommentModel> GetCommentsByUserId(string userId)
        {
            return _db.Comments.Include(c => c.User).Where(r => r.User.Id == userId).OrderBy(x => x.Recipe.Name).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateComment(CommentModel comment)
        {
            _db.Comments.Update(comment);
            return Save();
        }
    }
}
