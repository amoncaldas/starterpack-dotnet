using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using StarterPack.Core.Persistence;

namespace StarterPack.Models
{
    public class Role : Model<Role>, IEquatable<Role>
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(30)]
        public string Slug { get; set; }

        [JsonIgnore]
        public List<UserRole> UserRoles { get; set; }

        [NotMapped, JsonIgnore]
        public override DateTimeOffset? CreatedAt { get; set; }

        [NotMapped, JsonIgnore]
        public override DateTimeOffset? UpdatedAt { get; set; }

        public Role() {}

        public Role(long? id) {
            this.Id = id;
        }

        bool IEquatable<Role>.Equals(Role other)
        {
            return null != other && Id == other.Id;
        }
    }
}
