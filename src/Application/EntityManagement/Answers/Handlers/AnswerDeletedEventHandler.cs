using Application.EntityManagement.Answers.Events;
using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.EntityManagement.Answers.Handlers;

public class AnswerDeletedEventHandler : INotificationHandler<AnswerDeletedEvent>
{
    private readonly IRepository<Vote> _repository;

    public AnswerDeletedEventHandler(IRepository<Vote> repository)
    {
        _repository = repository;
    }

    public async Task Handle(AnswerDeletedEvent notification, CancellationToken cancellationToken)
    {
        var pagination = new Pagination(1, int.MaxValue);

        var votes = (await _repository.GetAllAsync(
                vote => vote.ContentId == notification.Entity.InternalId &&
                        vote.ContentType == VotableContentType.Answer,
                pagination,
                cancellationToken))
            .ToList();

        if (votes.Count == 0)
        {
            return;
        }

        await _repository.DeleteManyAsync(votes, cancellationToken);
    }
}