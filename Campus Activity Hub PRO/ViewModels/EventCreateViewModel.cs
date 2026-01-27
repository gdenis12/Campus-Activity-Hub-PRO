using Campus_Activity_Hub_PRO.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Campus_Activity_Hub_PRO.ViewModels
{
    public class EventCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Display(Name = "Poster URL")]
        public string PosterPath { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
