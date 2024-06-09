using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mottu.Domain.Entities;

namespace Mottu.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Motorcycle> Motorcycles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    EntityEntry Entry(object entity);
}
