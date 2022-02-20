using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models
{
    public class RecipeIngredientModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Order { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public RecipeModel Recipe { get; set; }
    }
}
