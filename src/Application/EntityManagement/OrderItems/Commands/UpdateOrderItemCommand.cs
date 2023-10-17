using Application.Common.Commands;
using Application.EntityManagement.OrderItems.Dtos;

namespace Application.EntityManagement.OrderItems.Commands;

public record UpdateOrderItemCommand(int Id, OrderItemDto OrderItemDto) : BaseUpdateCommand<OrderItemDto>(Id, OrderItemDto);