using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models.Dtos
{
    public class LikeCreateDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsLiked { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        public int RecipeId { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
