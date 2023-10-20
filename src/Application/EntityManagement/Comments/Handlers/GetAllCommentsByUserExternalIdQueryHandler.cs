using Application.Abstractions;
using Application.Common;
using Application.EntityManagement.Comments.Dtos;
using Application.EntityManagement.Comments.Queries;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.EntityManagement.Comments.Handlers;

public class GetAllCommentsByUserExternalIdQueryHandler : IRequestHandler<GetAllCommentsByUserExternalIdQuery, QueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly ILogger _logger;

    public GetAllCommentsByUserExternalIdQueryHandler(IUnitOfWork unitOfWork, IMappingService mappingService, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _logger = logger;
    }

    public async Task<QueryResponse> Handle(GetAllCommentsByUserExternalIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByExternalIdAsync(request.UserExternalId, new Func<User, object?>[]
            {
                entity => entity.Comments
            },
            cancellationToken);

        if (user is null)
        {
            return new QueryResponse
                (
                null,
                false,
                Messages.NotFound,
                HttpStatusCode.NotFound
                );
        }

        if (user.Comments is null)
        {
            _logger.LogError(Messages.EntityRelationshipsRetrievalFailed, DateTime.UtcNow, typeof(User), typeof(GetAllCommentsByUserExternalIdQueryHandler));

            return new QueryResponse
                (
                null,
                false,
                Messages.InternalServerError,
                HttpStatusCode.InternalServerError
                );
        }

        var commentDtos = _mappingService.Map<ICollection<Comment>, ICollection<CommentDto>>(user.Comments);

        if (commentDtos is not null)
        {
            return new QueryResponse
                (
                commentDtos.Paginate(request.Pagination),
                true,
                Messages.SuccessfullyRetrieved,
                HttpStatusCode.OK
                );
        }

        _logger.LogError(Messages.MappingFailed, DateTime.UtcNow, typeof(ICollection<Comment>), typeof(GetAllCommentsByUserExternalIdQueryHandler));

        return new QueryResponse
            (
            null,
            false,
            Messages.InternalServerError,
            HttpStatusCode.InternalServerError
            );
    }
}