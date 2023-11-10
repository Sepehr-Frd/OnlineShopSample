using Application.Common.Handlers;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.EntityManagement.Questions.Handlers;

public class GetAllQuestionsQueryHandler : BaseGetAllQueryHandler<Question>
{
    public GetAllQuestionsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}