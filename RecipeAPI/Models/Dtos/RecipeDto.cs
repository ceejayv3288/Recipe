﻿using RecipeAPI.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models.Dtos
{
    public class RecipeDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public UserModel User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        public int DurationInMin { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public CourseTypeEnum CourseType { get; set; }
    }
}
