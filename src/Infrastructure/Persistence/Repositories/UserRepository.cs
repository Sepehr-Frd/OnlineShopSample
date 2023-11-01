using Domain.Entities;
using Infrastructure.Persistence.Common;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(IOptions<MongoDbSettings> databaseSettings) : base(databaseSettings)
    {
    }
}