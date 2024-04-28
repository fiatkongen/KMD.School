using KMD.School.Model;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KMD.School.Infrastucture.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Student");
        modelBuilder.Entity<Student>().Property(x => x.Id).ValueGeneratedNever();
        modelBuilder.Entity<Student>().Property(x => x.FirstName).HasMaxLength(100);
        modelBuilder.Entity<Student>().Property(x => x.LastName).HasMaxLength(100);
        modelBuilder.Entity<Student>().Property(x => x.Address).HasMaxLength(100);
    }
}