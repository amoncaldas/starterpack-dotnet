using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using StarterPack.Core.Persistence;

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

        [Required]
        public DateTimeOffset? ScheduledTo { get; set; }

        [Required]
        public Project Project { get; set; }

        public long? ProjectId { get; set; }

        public Task() {
             this.Fill = new List<string>() { "*" };
             this.DontFill = new List<string>() { "Project" };
        }
    }
}
