using Application.Common.Handlers;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.EntityManagement.Votes.Handlers;

public class GetAllVotesQueryHandler : BaseGetAllQueryHandler<Vote>
{
    public GetAllVotesQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}