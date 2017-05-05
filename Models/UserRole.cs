using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using StarterPack.Core.Models;

namespace StarterPack.Models
{
    public class UserRole : Model<UserRole>
    {
        public long? UserId { get; set; }
        public User User { get; set; }

        public long? RoleId { get; set; }
        public Role Role { get; set; }

        [NotMapped]
        public override long? Id { get; set; }

        [NotMapped, JsonIgnore]
        public override DateTime? CreatedAt { get; set; }

        [NotMapped, JsonIgnore]
        public override DateTime? UpdatedAt { get; set; }

        public UserRole() {}

        public UserRole(Role role) {
            this.Role = role;
        }

        public UserRole(long? roleId) {
            this.RoleId = roleId;
        }        
    }
}