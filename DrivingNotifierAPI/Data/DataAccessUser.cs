using DrivingNotifierAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrivingNotifierAPI.Data
{
    public class DataAccessUser
    {

        private MongoClient client;
        private IMongoDatabase db;

        public DataAccessUser()
        {
            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("DrivingNotifier");
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await db.GetCollection<User>("Users").Find(_ => true).ToListAsync();
        }

        public User GetUser(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            return  db.GetCollection<User>("Users").Find(filter).First();
        }

        public  User GetUserByPhone(string phone)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Phone, phone);
            return db.GetCollection<User>("Users").Find(filter).First();
        }

        public async Task InsertUser(User user)
        {
            await db.GetCollection<User>("Users").InsertOneAsync(user);
        }

        public async Task UpdateUserPlayerID(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Phone, user.Phone);
            var update = Builders<User>.Update.Set(s => s.PlayerID, user.PlayerID);
            await db.GetCollection<User>("Users").UpdateOneAsync(filter, update);
        }

        public async Task DeleteUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Phone, user.Phone);
            await db.GetCollection<User>("Users").DeleteOneAsync(filter);
        }
    }
}
