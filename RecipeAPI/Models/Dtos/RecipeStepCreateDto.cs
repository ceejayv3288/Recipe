using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models.Dtos
{
    public class RecipeStepCreateDto
    {
        [Required]
        public int StepId { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        public int RecipeId { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}
