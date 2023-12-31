using Application.Common;
using MediatR;

namespace Application.EntityManagement.Questions.Commands;

public record DeleteQuestionByExternalIdCommand(int ExternalId) : IRequest<CommandResult>;