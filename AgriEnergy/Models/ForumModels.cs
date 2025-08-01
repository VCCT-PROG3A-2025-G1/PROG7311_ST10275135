﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergy.Models
{
    public class ForumModels
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string Author { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
