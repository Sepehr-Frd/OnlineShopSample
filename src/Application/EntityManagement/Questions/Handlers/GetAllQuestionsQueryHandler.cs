using System.Net;
using Application.Common;
using Application.Common.Constants;
using Application.EntityManagement.Questions.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.Questions.Handlers;

public sealed class
    GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, QueryResponse<IEnumerable<Question>>>
{
    private readonly IRepository<Question> _repository;

    public GetAllQuestionsQueryHandler(IRepository<Question> repository)
    {
        _repository = repository;
    }

    public async Task<QueryResponse<IEnumerable<Question>>> Handle(GetAllQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(request.Filter, cancellationToken);

        return new QueryResponse<IEnumerable<Question>>(
            entities,
            true,
            MessageConstants.SuccessfullyRetrieved,
            HttpStatusCode.OK);
    }
}