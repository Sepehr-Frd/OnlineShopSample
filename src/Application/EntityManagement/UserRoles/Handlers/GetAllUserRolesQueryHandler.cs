﻿using Application.Common;
using Application.Common.Constants;
using Application.EntityManagement.UserRoles.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.EntityManagement.UserRoles.Handlers;

public sealed class GetAllUserRolesQueryHandler : IRequestHandler<GetAllUserRolesQuery, QueryResponse<IEnumerable<UserRole>>>
{
    private readonly IRepository<UserRole> _repository;

    public GetAllUserRolesQueryHandler(IRepository<UserRole> repository)
    {
        _repository = repository;
    }

    public async Task<QueryResponse<IEnumerable<UserRole>>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(request.Filter, request.Pagination, cancellationToken);

        return new QueryResponse<IEnumerable<UserRole>>(
            entities,
            true,
            MessageConstants.SuccessfullyRetrieved,
            HttpStatusCode.OK);
    }
}