

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StarterPack.Models
{
    public class Role : Model<Role>
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Slug { get; set; }    

        public List<UserRole> UserRoles { get; set; }

        [NotMapped]
        public new DateTime CreatedAt { get; set; }

        [NotMapped]
        public new DateTime UpdatedAt { get; set; }        
    }
}