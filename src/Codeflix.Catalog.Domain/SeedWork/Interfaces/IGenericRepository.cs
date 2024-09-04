namespace Codeflix.Catalog.Domain.SeedWork.Interfaces;

public interface IGenericRepository<TAggregate> : IRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
    public Task<TAggregate> Get(Guid id, CancellationToken cancellationToken);
}
