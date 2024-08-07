using System.Linq.Expressions;
using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.Answers.Queries;

public record GetAllAnswersQuery(Expression<Func<Answer, bool>>? Filter = null)
    : IRequest<QueryResponse<IEnumerable<Answer>>>;