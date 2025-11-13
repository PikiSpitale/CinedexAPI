using Microsoft.EntityFrameworkCore;
using proyecto_prog4.Models.Genres;
using proyecto_prog4.Models.Movie;
using proyecto_prog4.Models.MovieGenres;
using proyecto_prog4.Models.Rol;
using proyecto_prog4.Models.UserFavorite;
using proyecto_prog4.Models.Usuario;

namespace proyecto_prog4.Config
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenres> MovieGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Rol>().HasIndex(x => x.Nombre).IsUnique();
            modelBuilder.Entity<UserFavorite>()
                .HasKey(x => new { x.UsuarioId, x.MovieId });
            modelBuilder.Entity<MovieGenres>()
                .HasKey(x => new { x.GenreId, x.MovieId });


            modelBuilder.Entity<Usuario>()
                .HasMany(x => x.Roles)
                .WithMany()
                .UsingEntity<RolUsuario>(
                    r => r.HasOne<Rol>().WithMany().HasForeignKey(x => x.RolId),
                    l => l.HasOne<Usuario>().WithMany().HasForeignKey(x => x.UsuarioId)
                );
            modelBuilder.Entity<UserFavorite>()
                .HasOne(umf => umf.Usuario)
                .WithMany(u => u.Favorites)
                .HasForeignKey(umf => umf.UsuarioId);

            modelBuilder.Entity<UserFavorite>()
                .HasOne(umf => umf.Movie)
                .WithMany(m => m.Favoritos)
                .HasForeignKey(umf => umf.MovieId);

            modelBuilder.Entity<MovieGenres>()
                .HasOne(g => g.Genre)
                .WithMany(m => m.Movies)
                .HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<MovieGenres>()
                .HasOne(m => m.Movie)
                .WithMany(g => g.Genres)
                .HasForeignKey(x => x.MovieId);

        }
    }
}


