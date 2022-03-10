using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public UserModel User { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [Required]
        public int RecipeId { get; set; }

        public RecipeModel Recipe { get; set; }
    }
}
