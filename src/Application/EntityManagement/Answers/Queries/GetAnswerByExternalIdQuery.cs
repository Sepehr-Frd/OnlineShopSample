using Application.Common.Queries;

namespace Application.EntityManagement.Answers.Queries;

public record GetAnswerByExternalIdQuery(int Id)
    : BaseGetByExternalIdQuery(Id);