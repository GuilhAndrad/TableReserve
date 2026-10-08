using Microsoft.EntityFrameworkCore;
using TableReserve.Domain.Entities;

namespace TableReserve.Infrastructure.DataAccess;

internal sealed class TableReserveDbContext(DbContextOptions<TableReserveDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; private set; } = null!;
    public DbSet<Table> Tables { get; private set; } = null!;
    public DbSet<Reservation> Reservations { get; private set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RefreshToken>().HasOne<User>().WithMany().HasForeignKey(refreshToken => refreshToken.UserId);
    }
}