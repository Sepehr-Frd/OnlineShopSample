using Application.Abstractions;
using Application.Common.Handlers;
using Application.EntityManagement.OrderItems.Dtos;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.EntityManagement.OrderItems.Handlers;

public class UpdateOrderItemCommandHandler : BaseUpdateCommandHandler<OrderItem, OrderItemDto>
{
    public UpdateOrderItemCommandHandler(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger logger) : base(unitOfWork, mappingService, logger)
    {
    }
}