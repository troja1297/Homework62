using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class Institution
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PhotoPath { get; set; }

        public IEnumerable<Dish> Dishes { get; set; }
    }
}
