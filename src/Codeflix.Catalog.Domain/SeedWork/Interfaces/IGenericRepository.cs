namespace Codeflix.Catalog.Domain.SeedWork.Interfaces;

public interface IGenericRepository<TAggregate> : IRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
}
