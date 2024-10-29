using Fall2024_Assignment3_akspencer1.Models;
using Microsoft.EntityFrameworkCore;

namespace Fall2024_Assignment3_akspencer1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Movie>()
        .HasKey(m => m.Title);

        modelBuilder.Entity<Actor>()
            .HasMany(a => a.Movies)
            .WithMany(m => m.Actors)
            .UsingEntity(j => j.ToTable("MovieActors"))
            .HasKey(m => m.Name);
            
    }

}

