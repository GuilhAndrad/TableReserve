using Microsoft.EntityFrameworkCore;
using TableReserve.Domain.Enums;
using TableReserve.Domain.Repositories.Reservation;

namespace TableReserve.Infrastructure.DataAccess.Repositories;

internal sealed class ReservationRepository(TableReserveDbContext dbContext) : IReservationWriter, IReservationReader
{
    private readonly TableReserveDbContext _dbContext = dbContext;

    public async Task Add(Domain.Entities.Reservation reservation, CancellationToken cancellationToken) =>
        await _dbContext.Reservations.AddAsync(reservation, cancellationToken);

    public async Task<Domain.Entities.Reservation?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Reservations.SingleOrDefaultAsync(reservation => reservation.Active && reservation.Id == id, cancellationToken);
    }

    public async Task<List<Domain.Entities.Reservation>> GetAllByUser(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Reservations
            .AsNoTracking()
            .Where(reservation => reservation.Active && reservation.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistOverlappingActiveReservation(Guid tableId, DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        return await _dbContext.Reservations.AnyAsync(reservation =>
            reservation.Active &&
            reservation.TableId == tableId &&
            reservation.Status == ReservationStatus.Active &&
            reservation.ReservationDate < end &&
            start < reservation.ReservationDate.AddMinutes(reservation.DurationMinutes), cancellationToken);
    }
}
