using Application.Abstractions;
using Application.Common;
using Application.EntityManagement.Questions.Commands;
using Application.EntityManagement.Questions.Dtos;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EntityManagement.Questions.Handlers;

public class CreateQuestionCommandHandler(
        IRepository<Question> repository,
        IMappingService mappingService,
        ILogger logger)
    : IRequestHandler<CreateQuestionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = mappingService.Map<QuestionDto, Question>(request.QuestionDto);

        if (entity is null)
        {
            logger.LogError(message: Messages.MappingFailed, DateTime.UtcNow, typeof(Question), typeof(CreateQuestionCommandHandler));

            return CommandResult.Failure(Messages.InternalServerError);
        }

        var createdEntity = await repository.CreateAsync(entity, cancellationToken);

        if (createdEntity is not null)
        {
            return CommandResult.Success(Messages.SuccessfullyCreated);
        }

        logger.LogError(message: Messages.EntityCreationFailed, DateTime.UtcNow, typeof(Question), typeof(CreateQuestionCommandHandler));

        return CommandResult.Failure(Messages.InternalServerError);
    }
}