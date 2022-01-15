using System;
using System.ComponentModel.DataAnnotations;

namespace Recipe.Models.Dtos
{
    public class LikeUpdateDto
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsLiked { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        public int RecipeId { get; set; }
    }
}
