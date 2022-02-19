using Recipe.Models;
using System.Collections.Generic;

namespace Recipe.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetComments();
        Comment GetComment(int commentId);
        bool CommentExists(string description);
        bool CommentExists(int id);
        bool CreateComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(Comment comment);
        bool Save();
    }
}
