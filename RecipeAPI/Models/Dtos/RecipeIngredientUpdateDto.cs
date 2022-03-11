using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Models.Dtos
{
    public class RecipeIngredientUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [Required]
        public int RecipeId { get; set; }
    }
}
