using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.EntityManagement.Users.Queries;

public sealed record GetUserByInternalIdQuery(Guid InternalId) : IRequest<QueryResponse<User>>;