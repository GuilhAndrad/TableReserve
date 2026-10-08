using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TableReserve.Domain.Entities;
using TableReserve.Domain.Repositories.RefreshToken;

namespace TableReserve.Infrastructure.DataAccess.Repositories;

internal sealed class RefreshTokenRepository(TableReserveDbContext dbContext) : IRefreshTokenRead, IRefreshTokenWrite
{
    private readonly TableReserveDbContext _dbContext = dbContext;
    
    public async Task Replace(RefreshToken refreshToken)
    {
        var existingToken = await _dbContext.RefreshTokens.Where(token => token.UserId == refreshToken.UserId).ToListAsync();

        _dbContext.RefreshTokens.RemoveRange(existingToken);
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> Get(string refreshToken)
    {
        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(token => token.Active && token.Value.Equals(refreshToken));
    }
}