using Antopia.Domain.Entities.LoginE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Domain.Entities.UserE;
using Microsoft.EntityFrameworkCore;

namespace Antopia.Infrastructure
{
    public class AntopiaDbContext : DbContext
    {
        private readonly string _connection;

        public AntopiaDbContext(string connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        // Login
        public virtual DbSet<LoginE> LoginEs { get; set; }
        public virtual DbSet<HistorialrefreshtokenE> HistorialrefreshtokenEs { get; set; }
        public virtual DbSet<CodigoRestablecimientoE> CodigoRestablecimientoEs { get; set; }

        // User 
        public virtual DbSet<UserE> UserEs { get; set; }

        // Publication
        public virtual DbSet<PublicationE> PublicationEs { get; set; }
        public virtual DbSet<PublicationImageE> PublicationImageEs { get; set; }
        public virtual DbSet<PublicationVideoE> PublicationVideoEs { get; set; }


    }
}