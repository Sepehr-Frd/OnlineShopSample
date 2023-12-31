using Application.EntityManagement.Questions.Queries;
using Application.EntityManagement.Users.Queries;
using Application.EntityManagement.Votes.Queries;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Web.GraphQL.Types;

public class AnswerType : ObjectType<Answer>
{
    protected override void Configure(IObjectTypeDescriptor<Answer> descriptor)
    {
        descriptor
            .Description("Represents an answer to a question, including details such as the content of the answer and associated user information.");

        descriptor
            .Field(answer => answer.InternalId)
            .Ignore();

        descriptor
            .Field(answer => answer.UserId)
            .Ignore();

        descriptor
            .Field(answer => answer.QuestionId)
            .Ignore();

        descriptor
            .Field(answer => answer.ExternalId)
            .Description("The External ID for Client Interactions");

        descriptor
            .Field(answer => answer.Description)
            .Description("The Body of an Answer");

        descriptor
            .Field(answer => answer.Question)
            .Description("The Original Question of This Answer")
            .ResolveWith<Resolvers>(
                resolvers =>
                    Resolvers.GetQuestionAsync(default!, default!));

        descriptor
            .Field(answer => answer.Title)
            .Description("The Answer Title");

        descriptor
            .Field(answer => answer.User)
            .Description("The User Who Posted This Answer")
            .ResolveWith<Resolvers>(
                resolvers =>
                    Resolvers.GetUserAsync(default!, default!));

        descriptor
            .Field(answer => answer.Votes)
            .Description("The Votes Submitted for This Answer")
            .ResolveWith<Resolvers>(
                resolvers =>
                    Resolvers.GetVotesAsync(default!, default!));

        descriptor
            .Field(answer => answer.DateCreated)
            .Description("The Creation Date");

        descriptor
            .Field(answer => answer.DateModified)
            .Description("The Last Modification Date");

        descriptor
            .Field(answer => answer.ExternalId)
            .Description("The External ID for Client Interactions");
    }

    private sealed class Resolvers
    {
        public static async Task<Question?> GetQuestionAsync([Parent] Answer answer, [Service] ISender sender)
        {
            var questionsQuery = new GetAllQuestionsQuery(
                new Pagination(),
                x => x.InternalId == answer.QuestionId);

            var result = await sender.Send(questionsQuery);

            return result.Data?.FirstOrDefault();
        }

        public static async Task<User?> GetUserAsync([Parent] Answer answer, [Service] ISender sender)
        {
            var usersQuery = new GetAllUsersQuery(
                new Pagination(),
                x => x.InternalId == answer.UserId);

            var result = await sender.Send(usersQuery);

            return result.Data?.FirstOrDefault();
        }

        public static async Task<IEnumerable<Vote>?> GetVotesAsync([Parent] Answer answer, [Service] ISender sender)
        {
            var votesQuery = new GetAllVotesQuery(
                new Pagination(),
                x => x.ContentId == answer.InternalId && x.Content is Answer);

            var result = await sender.Send(votesQuery);

            return result.Data;
        }
    }
}