using Application.EntityManagement.OrderItems.Dtos;
using Application.EntityManagement.Payments.Dtos;
using Application.EntityManagement.Shipments.Dtos;

namespace Application.EntityManagement.Orders.Dtos;

public record OrderDto
(
    int ExternalId,
    decimal TotalPrice,
    PaymentDto PaymentDto,
    ShipmentDto ShipmentDto,
    ICollection<OrderItemDto> OrderItemDtos
);