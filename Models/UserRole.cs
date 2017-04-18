using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarterPack.Models
{
    public class UserRole : Model<UserRole>
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long RoleId { get; set; }
        public Role Role { get; set; }

        [NotMapped]
        public new long Id { get; set; }

        [NotMapped]
        public new DateTime CreatedAt { get; set; }

        [NotMapped]
        public new DateTime UpdatedAt { get; set; }
    }
}