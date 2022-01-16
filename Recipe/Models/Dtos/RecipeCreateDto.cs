﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Recipe.Models.Dtos
{
    public class RecipeCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int DurationInMin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}