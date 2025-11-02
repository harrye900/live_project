using MongoDB.Driver;
using UserService.Models;

namespace UserService.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<User> _users;

        public MongoDbService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            var database = client.GetDatabase("DatingApp");
            _users = database.GetCollection<User>("Users");
        }

        public async Task<List<User>> GetUsersAsync() => 
            await _users.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserAsync(string id) =>
            await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _users.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateUserAsync(string id, User user) =>
            await _users.ReplaceOneAsync(x => x.Id == id, user);
    }
}