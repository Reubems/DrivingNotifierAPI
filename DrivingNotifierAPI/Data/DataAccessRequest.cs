using DrivingNotifierAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingNotifierAPI.Data
{
    public class DataAccessRequest
    {
        private MongoClient client;
        private IMongoDatabase db;

        public DataAccessRequest()
        {

            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("DrivingNotifier");

        }

        public async Task<IEnumerable<Request>> GetRequests()
        {
            return await db.GetCollection<Request>("Requests").Find(_ => true).ToListAsync();
        }

        public Request GetRequest(string requestorPhone, string replierPhone)
        {
            var filter = Builders<Request>.Filter.And(Builders<Request>.Filter.Eq(u => u.ReplierPhone, replierPhone), Builders<Request>.Filter.Eq(u => u.RequestorPhone, requestorPhone));
            return db.GetCollection<Request>("Requests").Find(filter).First();
        }

        public async Task CreateRequest(Request request)
        {
            await db.GetCollection<Request>("Requests").InsertOneAsync(request);
        }

        public async Task<IEnumerable<Request>> GetPendingRequests(string phone)
        {
            var filter = Builders<Request>.Filter.Eq(u => u.ReplierPhone, phone);
            return await db.GetCollection<Request>("Requests").Find(filter).ToListAsync();
        }

        public async Task UpdateRequestState(string requestorPhone, string replierPhone, RequestState state)
        {
            var filter = Builders<Request>.Filter.And(Builders<Request>.Filter.Eq(u => u.ReplierPhone, replierPhone), Builders<Request>.Filter.Eq(u => u.RequestorPhone, requestorPhone));
            var update = Builders<Request>.Update.Set(s => s.State, state);
            await db.GetCollection<Request>("Requests").UpdateOneAsync(filter, update);

            //TODO: Depending on the state, we update the property Contacts of the requestor adding the replier's ObjectId with add().
        }

        public async Task DeleteRequest(string requestorPhone, string replierPhone)
        {
            var filter = Builders<Request>.Filter.And(Builders<Request>.Filter.Eq(u => u.ReplierPhone, replierPhone), Builders<Request>.Filter.Eq(u => u.RequestorPhone, requestorPhone));
            await db.GetCollection<Request>("Requests").DeleteOneAsync(filter);
        }

    }
}
