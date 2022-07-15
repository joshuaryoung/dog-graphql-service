namespace hot_chocolate_demo.GraphQL;
using Microsoft.EntityFrameworkCore;

public class DogDataContext : DbContext {
  public DogDataContext(DbContextOptions<DogDataContext> options): base(options) {
    options.ToString();
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.UseSerialColumns();
  }

  public DbSet<Dog>? dog { get; set; }
  public DbSet<User>? user { get; set; }
}