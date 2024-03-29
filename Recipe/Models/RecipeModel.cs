﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Recipe.Models
{
    public class RecipeModel
    {
        [Key]
        public int Id { get; set; }
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
