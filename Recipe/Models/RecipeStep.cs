using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe.Models
{
    public class RecipeStep
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public RecipeModel Recipe { get; set; }
    }
}
