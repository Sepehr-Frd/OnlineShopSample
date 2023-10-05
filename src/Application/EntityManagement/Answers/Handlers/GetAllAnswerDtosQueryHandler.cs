using Application.Abstractions;
using Application.Common.Handlers;
using Application.EntityManagement.Answers.Dtos;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.EntityManagement.Answers.Handlers;

public class GetAllAnswerDtosQueryHandler : BaseGetAllDtosQueryHandler<Answer, AnswerDto>
{
    public GetAllAnswerDtosQueryHandler(IRepository<Answer> repository, IMappingService mappingService) : base(repository, mappingService)
    {
    }
}