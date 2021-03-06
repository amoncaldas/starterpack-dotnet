using Microsoft.EntityFrameworkCore;
using StarterPack.Models;

namespace StarterPack.Core.Persistence
{
    public partial class DefaultDbContext
    {
        // Todos os models devem ser adicionados ao DbSet aqui para poderem ser transacionados com o banco
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// Aqui dvem ser configurados os models, especificando restrições e relacionamentos que não pode ser
        /// configurado através de anotações.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigureModels(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(e => e.Salt)
                .HasField("_salt");

            modelBuilder.Entity<Project>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(e => e.Slug)
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        }

    }
}
