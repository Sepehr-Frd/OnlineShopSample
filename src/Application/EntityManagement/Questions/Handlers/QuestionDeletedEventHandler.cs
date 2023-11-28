using Application.EntityManagement.Questions.Events;
using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.Questions.Handlers;

public class QuestionDeletedEventHandler : INotificationHandler<QuestionDeletedEvent>
{
    private readonly IRepository<Answer> _answerRepository;
    private readonly IRepository<Vote> _voteRepository;

    public QuestionDeletedEventHandler(IRepository<Answer> answerRepository, IRepository<Vote> voteRepository)
    {
        _answerRepository = answerRepository;
        _voteRepository = voteRepository;
    }

    public async Task Handle(QuestionDeletedEvent notification, CancellationToken cancellationToken)
    {
        var pagination = new Pagination(1, int.MaxValue);

        var votes = (await _voteRepository.GetAllAsync(
                vote => vote.ContentId == notification.Entity.InternalId && vote.Content is Question,
                pagination,
                cancellationToken))
            .ToList();

        if (votes.Count != 0)
        {
            await _voteRepository.DeleteManyAsync(votes, cancellationToken);
        }

        var answers = (await _answerRepository.GetAllAsync(
                answer => answer.QuestionId == notification.Entity.InternalId,
                pagination,
                cancellationToken))
            .ToList();

        if (answers.Count != 0)
        {
            await _answerRepository.DeleteManyAsync(answers, cancellationToken);

            var answerInternalIds = answers.Select(answer => answer.InternalId).ToList();

            var answerVotes = (await _voteRepository.GetAllAsync(
                    vote => answerInternalIds.Contains(vote.ContentId) &&
                            vote.Content is Answer,
                    pagination,
                    cancellationToken))
                .ToList();

            if (answerVotes.Count != 0)
            {
                await _voteRepository.DeleteManyAsync(answerVotes, cancellationToken);
            }
        }
    }
}