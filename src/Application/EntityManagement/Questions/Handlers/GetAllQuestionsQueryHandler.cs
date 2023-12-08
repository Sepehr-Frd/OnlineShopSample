using Application.Common;
using Application.EntityManagement.Questions.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.EntityManagement.Questions.Handlers;

public sealed class GetAllQuestionsQueryHandler(IRepository<Question> repository)
    : IRequestHandler<GetAllQuestionsQuery, QueryReferenceResponse<IEnumerable<Question>>>
{
    public async Task<QueryReferenceResponse<IEnumerable<Question>>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(request.Filter, request.Pagination, cancellationToken);

        return new QueryReferenceResponse<IEnumerable<Question>>(
            entities,
            true,
            MessageConstants.SuccessfullyRetrieved,
            HttpStatusCode.OK);
    }
}