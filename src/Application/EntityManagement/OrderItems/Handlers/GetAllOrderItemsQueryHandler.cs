using System.Net;
using Application.Common;
using Application.Common.Constants;
using Application.EntityManagement.OrderItems.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.OrderItems.Handlers;

public sealed class
    GetAllOrderItemsQueryHandler : IRequestHandler<GetAllOrderItemsQuery, QueryResponse<IEnumerable<OrderItem>>>
{
    private readonly IRepository<OrderItem> _repository;

    public GetAllOrderItemsQueryHandler(IRepository<OrderItem> repository)
    {
        _repository = repository;
    }

    public async Task<QueryResponse<IEnumerable<OrderItem>>> Handle(GetAllOrderItemsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(request.Filter, cancellationToken);

        return new QueryResponse<IEnumerable<OrderItem>>(
            entities,
            true,
            MessageConstants.SuccessfullyRetrieved,
            HttpStatusCode.OK);
    }
}