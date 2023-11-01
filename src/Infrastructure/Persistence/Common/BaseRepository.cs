using Domain.Abstractions;
using Domain.Common;
using Infrastructure.Persistence.Common.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Common;

public class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly IMongoCollection<TEntity> _mongoDbCollection;

    public BaseRepository(IOptions<MongoDbSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _mongoDbCollection = mongoDatabase.GetCollection<TEntity>(databaseSettings.Value.CollectionName);
    }

    public async Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _mongoDbCollection.InsertOneAsync(entity, null, cancellationToken);

        var filterDefinition = Builders<TEntity>.Filter.Eq(document => document.InternalId, entity.InternalId);

        var documentCursor = await _mongoDbCollection.FindAsync<TEntity>(filterDefinition, cancellationToken: cancellationToken);

        return await documentCursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GenerateUniqueExternalIdAsync(CancellationToken cancellationToken = default)
    {
        var findOptions = new FindOptions<TEntity>
        {
            Sort = Builders<TEntity>.Sort.Descending(document => document.ExternalId),
            Limit = 1
        };

        var documentCursor = await _mongoDbCollection.FindAsync(FilterDefinition<TEntity>.Empty, findOptions, cancellationToken);

        var document = await documentCursor.FirstOrDefaultAsync(cancellationToken);

        return document is null ? 0 : document.ExternalId + 1;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object?>>[] includes)
    {
        var aggregate = _mongoDbCollection.Aggregate();

        if (filter is not null)
        {
            aggregate = aggregate.Match(filter);
        }

        aggregate.IncludeRelations(includes);

        var result = await aggregate.ToListAsync(cancellationToken);

        return result;
    }

    public async Task<TEntity?> GetByInternalIdAsync(Guid internalId, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object?>>[] includes)
    {
        var aggregate = _mongoDbCollection.Aggregate();

        var filterDefinition = new FilterDefinitionBuilder<TEntity>()
            .Eq(document => document.InternalId, internalId);

        aggregate.Match(filterDefinition);

        aggregate.IncludeRelations(includes);

        var result = await aggregate.FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<TEntity?> GetByExternalIdAsync(int externalId, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object?>>[] includes)
    {
        var aggregate = _mongoDbCollection.Aggregate();

        var filterDefinition = new FilterDefinitionBuilder<TEntity>()
            .Eq(document => document.ExternalId, externalId);

        aggregate.Match(filterDefinition);

        aggregate.IncludeRelations(includes);

        var result = await aggregate.FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var filterDefinition = new FilterDefinitionBuilder<TEntity>()
            .Eq(document => document.InternalId, entity.InternalId);

        var result = await _mongoDbCollection.FindOneAndReplaceAsync(filterDefinition, entity, null, cancellationToken);

        return result;
    }

    public async Task<TEntity?> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var filterDefinition = new FilterDefinitionBuilder<TEntity>()
            .Where(document => document.InternalId == entity.InternalId);

        var result = await _mongoDbCollection.FindOneAndDeleteAsync(filterDefinition, null, cancellationToken);

        return result;
    }
}