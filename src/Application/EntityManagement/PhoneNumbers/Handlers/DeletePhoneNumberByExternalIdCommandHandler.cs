using Application.Common;
using Application.EntityManagement.Answers.Commands;
using Application.EntityManagement.PhoneNumbers.Commands;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EntityManagement.PhoneNumbers.Handlers;

public class DeletePhoneNumberByExternalIdCommandHandler(IRepository<PhoneNumber> repository, ILogger logger)
    : IRequestHandler<DeletePhoneNumberByExternalIdCommand, CommandResult>
{
    public virtual async Task<CommandResult> Handle(DeletePhoneNumberByExternalIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        if (entity is null)
        {
            return CommandResult.Failure(Messages.NotFound);
        }

        var deletedEntity = await repository.DeleteAsync(entity, cancellationToken);

        if (deletedEntity is not null)
        {
            return CommandResult.Success(Messages.SuccessfullyDeleted);
        }

        logger.LogError(Messages.EntityDeletionFailed, DateTime.UtcNow, typeof(PhoneNumber), typeof(DeleteAnswerByExternalIdCommand));

        return CommandResult.Failure(Messages.InternalServerError);
    }
}