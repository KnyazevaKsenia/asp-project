using AspProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspProject.Infrastrastructure.Database;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Material> Materials { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.StudentId, f.MaterialId });
        
        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Student)
            .WithMany(s => s.Favorites)
            .HasForeignKey(f => f.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Material)
            .WithMany(m => m.Favorites)
            .HasForeignKey(f => f.MaterialId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Student)
            .WithOne(s => s.User)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
    }
}


