using Microsoft.EntityFrameworkCore;
using StarterPack.Models;

namespace StarterPack.Core.Persistence
{
    public partial class DatabaseContext
    {
        // Todos os models devem ser adicionados ao DbSet aqui para poderem ser transacionados com o banco
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}