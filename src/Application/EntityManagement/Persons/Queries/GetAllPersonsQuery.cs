using System.Linq.Expressions;
using Application.Common;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.Persons.Queries;

public record GetAllPersonsQuery(
        Pagination Pagination,
        Expression<Func<Person, object?>>[]? RelationsToInclude = null,
        Expression<Func<Person, bool>>? Filter = null)
    : IRequest<QueryReferenceResponse<IEnumerable<Person>>>;