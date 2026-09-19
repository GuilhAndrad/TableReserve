using Microsoft.EntityFrameworkCore;
using TableReserve.Domain.Entities;

namespace TableReserve.Infrastructure.DataAccess;

internal sealed class TableReserveDbContext(DbContextOptions<TableReserveDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}