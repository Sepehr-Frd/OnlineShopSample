using Application.Abstractions;
using Application.Common.Queries;
using Domain.Abstractions;
using Domain.Common;
using MediatR;

namespace Application.Common.Handlers;

public abstract class BaseGetAllDtosQueryHandler<TEntity, TDto>
    : IRequestHandler<BaseGetAllDtosQuery<TDto>, IEnumerable<TDto>?>
    where TEntity : BaseEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMappingService _mappingService;

    protected BaseGetAllDtosQueryHandler(IRepository<TEntity> repository, IMappingService mappingService)
    {
        _repository = repository;
        _mappingService = mappingService;
    }

    public async Task<IEnumerable<TDto>?> Handle(BaseGetAllDtosQuery<TDto> request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);

        var dtos = _mappingService.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(entities);

        return dtos;
    }
}