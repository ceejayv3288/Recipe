using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models.Dtos
{
    public class LikeCreateDto
    {
        public string UserId { get; set; }

        [Required]
        public bool IsLiked { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int RecipeId { get; set; }
    }
}
