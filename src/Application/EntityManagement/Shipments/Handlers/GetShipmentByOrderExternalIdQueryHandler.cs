using Application.Abstractions;
using Application.Common;
using Application.EntityManagement.Shipments.Dtos;
using Application.EntityManagement.Shipments.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.EntityManagement.Shipments.Handlers;

public class GetShipmentByOrderExternalIdQueryHandler : IRequestHandler<GetShipmentByOrderExternalIdQuery, QueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly ILogger _logger;

    public GetShipmentByOrderExternalIdQueryHandler(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _logger = logger;
    }

    public async Task<QueryResponse> Handle(GetShipmentByOrderExternalIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetByExternalIdAsync(request.OrderExternalId,
            new Func<Order, object?>[]
            {
                entity => entity.Shipment
            },
            cancellationToken);

        if (order is null)
        {
            return new QueryResponse
                (
                null,
                false,
                Messages.NotFound,
                HttpStatusCode.NotFound
                );
        }

        if (order.Shipment is null)
        {
            _logger.LogError(Messages.EntityRelationshipsRetrievalFailed, DateTime.UtcNow, typeof(Order), typeof(GetShipmentByOrderExternalIdQueryHandler));

            return new QueryResponse
                (
                null,
                false,
                Messages.InternalServerError,
                HttpStatusCode.InternalServerError
                );
        }

        var shipmentDto = _mappingService.Map<Shipment, ShipmentDto>(order.Shipment);

        if (shipmentDto is not null)
        {
            return new QueryResponse
                (
                shipmentDto,
                true,
                Messages.SuccessfullyRetrieved,
                HttpStatusCode.OK
                );
        }

        _logger.LogError(Messages.MappingFailed, DateTime.UtcNow, typeof(Shipment), typeof(GetShipmentByOrderExternalIdQueryHandler));

        return new QueryResponse
            (
            null,
            false,
            Messages.InternalServerError,
            HttpStatusCode.InternalServerError
            );
    }
}