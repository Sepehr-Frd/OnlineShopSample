using Application.Common;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Application.EntityManagement.CartItems.Queries;

public record GetAllCartItemsQuery(
        Pagination Pagination,
        Expression<Func<CartItem, bool>>? Filter = null)
    : IRequest<QueryResponse<IEnumerable<CartItem>>>;