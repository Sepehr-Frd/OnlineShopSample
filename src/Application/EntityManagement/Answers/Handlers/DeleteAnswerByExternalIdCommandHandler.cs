using Application.Common.Handlers;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.EntityManagement.Answers.Handlers;

public class DeleteAnswerByExternalIdCommandHandler : BaseDeleteByExternalIdCommandHandler<Answer>
{
    public DeleteAnswerByExternalIdCommandHandler(IUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger)
    {
    }
}