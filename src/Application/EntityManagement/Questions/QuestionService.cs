using Application.Common;
using Application.Common.Constants;
using Application.EntityManagement.Questions.Commands;
using Application.EntityManagement.Questions.Events;
using Application.EntityManagement.Questions.Queries;
using MediatR;

namespace Application.EntityManagement.Questions;

public class QuestionService
{
    private readonly IMediator _mediator;

    public QuestionService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<CommandResult> DeleteByExternalIdAsync(int externalId,
        CancellationToken cancellationToken = default)
    {
        var questionsQuery = new GetAllQuestionsQuery(question => question.ExternalId == externalId);

        var questionResult = await _mediator.Send(questionsQuery, cancellationToken);

        if (!questionResult.IsSuccessful ||
            questionResult.Data is null ||
            !questionResult.Data.Any())
        {
            return CommandResult.Failure(MessageConstants.NotFound);
        }

        var deleteQuestionCommand = new DeleteQuestionByExternalIdCommand(externalId);

        await _mediator.Send(deleteQuestionCommand, cancellationToken);

        var questionDeletedEvent = new QuestionDeletedEvent(questionResult.Data.First());

        await _mediator.Publish(questionDeletedEvent, cancellationToken);

        return CommandResult.Success(MessageConstants.SuccessfullyDeleted);
    }
}