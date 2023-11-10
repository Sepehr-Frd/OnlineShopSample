using Application.Common;
using Application.EntityManagement.Answers.Queries;
using Application.EntityManagement.Comments.Queries;
using Application.EntityManagement.Products.Queries;
using Application.EntityManagement.Questions.Queries;
using Application.EntityManagement.Users.Queries;
using Domain.Abstractions;
using Domain.Entities;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Web.GraphQL.Types;

public class VoteType : ObjectType<Vote>
{
    protected override void Configure(IObjectTypeDescriptor<Vote> descriptor)
    {
        descriptor
            .Field(vote => vote.User)
            .ResolveWith<Resolvers>(
                resolvers =>
                    resolvers.GetUserAsync(default!, default!));

        descriptor
            .Field(vote => vote.Content)
            .ResolveWith<Resolvers>(
                resolvers =>
                    resolvers.GetContentAsync(default!, default!));
        
        descriptor
            .Field(vote => vote.DateCreated)
            .Description("The Creation Date");

        descriptor
            .Field(vote => vote.DateModified)
            .Description("The Last Modification Date");

        descriptor
            .Field(vote => vote.ExternalId)
            .Description("The External ID for Client Interactions");

        descriptor
            .Field(vote => vote.InternalId)
            .Ignore();

        descriptor
            .Field(vote => vote.UserId)
            .Ignore();
        
        descriptor
            .Field(vote => vote.ContentId)
            .Ignore();
    }

    private sealed class Resolvers
    {
        public async Task<User?> GetUserAsync([Parent] Vote vote, [Service] ISender sender)
        {
            var usersQuery = new GetAllUsersQuery(
                new Pagination(),
                null,
                x => x.InternalId == vote.UserId);

            var result = await sender.Send(usersQuery);

            return result.Data?.FirstOrDefault();
        }
        
        public async Task<IVotableContent?> GetContentAsync([Parent] Vote vote, [Service] ISender sender)
        {
            if (vote.Content is Answer)
            {
                var contentsQuery = new GetAllAnswersQuery(
                    new Pagination(),
                    null,
                    x => x.InternalId == vote.ContentId);
                
                var result = await sender.Send(contentsQuery);

                return result.Data?.FirstOrDefault();
            }
             
            if (vote.Content is Comment)
            {
                var contentsQuery = new GetAllCommentsQuery(
                    new Pagination(),
                    null,
                    x => x.InternalId == vote.ContentId);
                
                var result = await sender.Send(contentsQuery);

                return result.Data?.FirstOrDefault();
            }
            
            if (vote.Content is Product)
            {
                var contentsQuery = new GetAllProductsQuery(
                    new Pagination(),
                    null,
                    x => x.InternalId == vote.ContentId);
                
                var result = await sender.Send(contentsQuery);

                return result.Data?.FirstOrDefault();
            }
            
            if (vote.Content is Question)
            {
                var contentsQuery = new GetAllQuestionsQuery(
                    new Pagination(),
                    null,
                    x => x.InternalId == vote.ContentId);
                
                var result = await sender.Send(contentsQuery);

                return result.Data?.FirstOrDefault();
            }

            return null;
        }
    }
}