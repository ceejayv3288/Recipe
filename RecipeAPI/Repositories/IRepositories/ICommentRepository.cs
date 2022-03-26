using RecipeAPI.Models;
using System.Collections.Generic;

namespace RecipeAPI.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        ICollection<CommentModel> GetComments();

        ICollection<CommentModel> GetCommentsByRecipeId(int recipeId);

        ICollection<CommentModel> GetCommentsByUserId(string userId);

        ICollection<CommentModel> GetCommentsByRecipeAndUserId(int recipeId, string userId);

        CommentModel GetComment(int commentId);

        bool CommentExists(string description);

        bool CommentExists(int id);

        bool CreateComment(CommentModel comment);

        bool UpdateComment(CommentModel comment);

        bool DeleteComment(CommentModel comment);

        bool Save();
    }
}
