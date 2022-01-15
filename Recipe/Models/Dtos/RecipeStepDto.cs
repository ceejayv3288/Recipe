﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Recipe.Models.Dtos
{
    public class RecipeStepDto
    {
        public int Id { get; set; }
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
