namespace Infrastructure.Database;

using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<Price> Price { get; set; }
    public DbSet<Parking> Parking { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
#if TEST
        SeedTest.OnModelCreating(builder);
#else
#endif
        base.OnModelCreating(builder);
    }
}
