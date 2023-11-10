using MediatR;
using System.Linq.Expressions;

namespace Application.Common.Queries;

public abstract record BaseGetAllQuery<TEntity>(
        Pagination Pagination,
        Expression<Func<TEntity, object?>>[]? RelationsToInclude = null,
        Expression<Func<TEntity, bool>>? Filter = null)
    : IRequest<QueryReferenceResponse<IEnumerable<TEntity>>>;