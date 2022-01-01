using Recipe.Models;
using Recipe.Repositories.IRepositories;
using System.Collections.Generic;

namespace Recipe.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        public bool CommentExists(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool CommentExists(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateComment(Comment comment)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteComment(Comment comment)
        {
            throw new System.NotImplementedException();
        }

        public Comment GetComment(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Comment> GetComments()
        {
            throw new System.NotImplementedException();
        }

        public bool Save()
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateComment(Comment comment)
        {
            throw new System.NotImplementedException();
        }
    }
}
