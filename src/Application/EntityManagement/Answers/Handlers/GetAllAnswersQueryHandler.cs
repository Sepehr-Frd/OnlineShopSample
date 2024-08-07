using System.Net;
using Application.Common;
using Application.Common.Constants;
using Application.EntityManagement.Answers.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.Answers.Handlers;

public sealed class GetAllAnswersQueryHandler : IRequestHandler<GetAllAnswersQuery, QueryResponse<IEnumerable<Answer>>>
{
    private readonly IRepository<Answer> _repository;

    public GetAllAnswersQueryHandler(IRepository<Answer> repository)
    {
        _repository = repository;
    }

    public async Task<QueryResponse<IEnumerable<Answer>>> Handle(GetAllAnswersQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(request.Filter, cancellationToken);

        return new QueryResponse<IEnumerable<Answer>>(
            entities,
            true,
            MessageConstants.SuccessfullyRetrieved,
            HttpStatusCode.OK);
    }
}