using Application.Abstractions;
using Application.Common;
using Application.EntityManagement.Payments.Dtos;
using Application.EntityManagement.Payments.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.EntityManagement.Payments.Handlers;

public class GetPaymentByOrderExternalIdQueryHandler : IRequestHandler<GetPaymentByOrderExternalIdQuery, QueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly ILogger _logger;

    public GetPaymentByOrderExternalIdQueryHandler(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _logger = logger;
    }

    public async Task<QueryResponse> Handle(GetPaymentByOrderExternalIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetByExternalIdAsync(request.OrderExternalId,
            new Func<Order, object?>[]
            {
                entity => entity.Payment
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

        if (order.Payment is null)
        {
            _logger.LogError(Messages.EntityRelationshipsRetrievalFailed, DateTime.UtcNow, typeof(Order), typeof(GetPaymentByOrderExternalIdQueryHandler));

            return new QueryResponse
                (
                null,
                false,
                Messages.InternalServerError,
                HttpStatusCode.InternalServerError
                );
        }

        var paymentDto = _mappingService.Map<Payment, PaymentDto>(order.Payment);

        if (paymentDto is not null)
        {
            return new QueryResponse
                (
                paymentDto,
                true,
                Messages.SuccessfullyRetrieved,
                HttpStatusCode.OK
                );
        }

        _logger.LogError(Messages.MappingFailed, DateTime.UtcNow, typeof(Payment), typeof(GetPaymentByOrderExternalIdQueryHandler));

        return new QueryResponse
            (
            null,
            false,
            Messages.InternalServerError,
            HttpStatusCode.InternalServerError
            );
    }
}