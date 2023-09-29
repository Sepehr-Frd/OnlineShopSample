using Application.Common.Queries;
using Domain.Abstractions;
using Domain.Common;
using MediatR;

namespace Application.Common.Handlers;

public class BaseGetByInternalIdHandler<TEntity>
    : IRequestHandler<BaseGetByInternalIdQuery<TEntity>, TEntity?>
    where TEntity : BaseEntity
{
    private readonly IRepository<TEntity> _repository;

    public BaseGetByInternalIdHandler(IRepository<TEntity> repository) =>
        _repository = repository;

    public async Task<TEntity?> Handle(BaseGetByInternalIdQuery<TEntity> request, CancellationToken cancellationToken) =>
        await _repository.GetByInternalIdAsync(request.Id, cancellationToken);
}