using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models.Dtos
{
    public class RecipeIngredientCreateDto
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int RecipeId { get; set; }

        public RecipeModel Recipe { get; set; }
    }
}
