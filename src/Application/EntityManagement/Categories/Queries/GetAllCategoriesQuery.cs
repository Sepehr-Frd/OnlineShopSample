using System.Linq.Expressions;
using Application.Common;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.Categories.Queries;

public record GetAllCategoriesQuery(
        Pagination Pagination,
        Expression<Func<Category, object?>>[]? RelationsToInclude = null,
        Expression<Func<Category, bool>>? Filter = null)
    : IRequest<QueryReferenceResponse<IEnumerable<Category>>>;