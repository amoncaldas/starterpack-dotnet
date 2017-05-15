using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using StarterPack.Core.Persistence;

namespace StarterPack.Models
{
    public class Project : Model<Project>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public double? Cost { get; set; }

        public List<Task> Tasks  { get; set; }

        public Project() {
             this.Fill = new List<string>() { "*" };
             this.DontFill = new List<string>() { "Tasks" };
        }
    }
}
