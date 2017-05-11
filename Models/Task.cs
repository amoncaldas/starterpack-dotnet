using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using StarterPack.Core.Models;

namespace StarterPack.Models
{
    public class Task : Model<Task>
    {
        [Required]
        public string Description { get; set; }
        
        [Required]
        public bool Done { get; set; }

        [Required]
        public int Priority { get; set; }

        public DateTime ScheduledTo { get; set; }

        public Project Project { get; set; }   
    }
}