using Application.Abstractions;
using Application.Common.Handlers;
using Application.EntityManagement.Payments.Dtos;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.EntityManagement.Payments.Handlers;

public class CreatePaymentCommandHandler : BaseCreateCommandHandler<Payment, PaymentDto>
{
    public CreatePaymentCommandHandler(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger logger) : base(unitOfWork, mappingService, logger)
    {
    }
}