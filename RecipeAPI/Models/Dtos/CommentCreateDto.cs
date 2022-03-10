﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models.Dtos
{
    public class CommentCreateDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [Required]
        public int RecipeId { get; set; }
    }
}
