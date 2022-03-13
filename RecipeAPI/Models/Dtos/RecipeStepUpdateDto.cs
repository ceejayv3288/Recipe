using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models.Dtos
{
    public class RecipeStepUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int RecipeId { get; set; }
    }
}
