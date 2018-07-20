using DrivingNotifierAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingNotifierAPI.Data
{
    public class DataAccessUser
    {
        private MongoClient client;
        private IMongoDatabase db;
        private readonly string DB_COLLECTION_NAME_USERS = "Users";
        private readonly string DB_NAME = "DrivingNotifier";
        //private readonly string DB_CLIENT_URL_LOCAL = "mongodb://localhost:27017";
        private readonly string DB_CLIENT_URL_REMOTE = "mongodb://dnadmin:"+ PrivateCredentials.PASS_DB_REMOTE + "@" +
            "drivingnotifier-shard-00-00-i0wld.mongodb.net:27017," +
            "drivingnotifier-shard-00-01-i0wld.mongodb.net:27017," +
            "drivingnotifier-shard-00-02-i0wld.mongodb.net:27017/test?" +
            "ssl=true&replicaSet=DrivingNotifier-shard-0&authSource=admin&retryWrites=true";

        public DataAccessUser()
        {
            client = new MongoClient(DB_CLIENT_URL_REMOTE);
            db = client.GetDatabase(DB_NAME);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).Find(_ => true).ToListAsync();
        }

        public User GetUser(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);

            return  db.GetCollection<User>(DB_COLLECTION_NAME_USERS).Find(filter).FirstOrDefault();
        }

        public  User GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);

            return db.GetCollection<User>(DB_COLLECTION_NAME_USERS).Find(filter).FirstOrDefault();
        }

        public async Task InsertUser(User user)
        {
            await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).InsertOneAsync(user);
        }

        public async Task UpdateUserPlayerID(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(s => s.PlayerID, user.PlayerID);

            await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserTrackingEnabledState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(s => s.TrackingEnabled, user.TrackingEnabled);

            await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserMuteState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(s => s.Mute, user.Mute);

            await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserDrivingState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.
                Set(s => s.Driving, user.Driving).
                Set(s => s.LastUpdate, DateTime.Now);

            await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).UpdateOneAsync(filter, update);
        }

        public async Task DeleteUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);

            await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).DeleteOneAsync(filter);
        }

        //Push notification

        public void PushNotification(string email) 
        {
            User user = GetUserByEmail(email);

            var contacts = user.Contacts;
            StringBuilder sb = new StringBuilder();

            foreach (ObjectId id in contacts ){
                User fetched = GetUser(id);
                if(fetched.Mute == false)
                {
                    string playerID = fetched.PlayerID;
                    string playerIDformatted = string.Format("\"{0}\",", playerID);
                    sb.Append(playerIDformatted);
                }    
            }
            sb.Remove(sb.Length - 1, 1); //To remove the last ",".

            var client = new RestClient("https://onesignal.com/api/v1/notifications");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic ZjMyN2JiMDctNDI4ZC00OWI1LWEyZTAtMjBjYTE5MjYzOTk4");
            request.AddParameter("undefined", "{\n\t\"app_id\" : \"29d6b657-f182-41e7-85cd-1807d337fdfc\" ," +
                "\n\t\"contents\" : { \"en\" : \"I am driving now!\" }," +
                "\n\t\"include_player_ids\" : ["+ sb +"]\n}", ParameterType.RequestBody);
            client.ExecuteAsync(request, (response) =>
            {
               Console.WriteLine(response.StatusCode); //TODO Change for Application Insights
            });
        }

        //Add to Contacts List
        public async Task AddUserContactList(string emailRequestor, string emailReplier)
        {
            User requestor = GetUserByEmail(emailRequestor);
            User replier = GetUserByEmail(emailReplier);
            if(requestor != null && replier != null)
            {
                var contacts = new List<ObjectId>(); //In case we save the first contact.

                if (replier.Contacts != null)
                {
                     contacts = replier.Contacts;
                }

                if (!contacts.Contains(requestor.Id))
                {
                    contacts.Add(requestor.Id); //We add the ObjectId
                }

                var filter = Builders<User>.Filter.Eq(u => u.Email, replier.Email);
                var update = Builders<User>.Update.Set(s => s.Contacts, contacts);
                await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).UpdateOneAsync(filter, update);
            }
        }

        //Remove from Contacts List
        public async Task RemoveUserContactList(string emailRequestor, string emailToDelete)
        {
            User requestor = GetUserByEmail(emailRequestor);
            User deleted = GetUserByEmail(emailToDelete);
            if (requestor != null && deleted != null && requestor.Contacts != null)
            {
                var contacts = requestor.Contacts;
                
                contacts.Remove(deleted.Id); //We remove the ObjectId

                var filter = Builders<User>.Filter.Eq(u => u.Email, requestor.Email);
                var update = Builders<User>.Update.Set(s => s.Contacts, contacts);

                await db.GetCollection<User>(DB_COLLECTION_NAME_USERS).UpdateOneAsync(filter, update);
            }
        }

        public List<string> GetContactsList(string phone)
        {
            User requestor = GetUserByEmail(phone);

            return requestor.Contacts != null ? requestor.Contacts.
                Select(e => GetUser(e).Email).ToList() : new List<string>();
        }

        public List<string> GetContactsDrivingList(string email)
        {
            User requestor = GetUserByEmail(email);

            return requestor.Contacts != null ? requestor.Contacts
                .Where(e => GetUser(e).TrackingEnabled == true)
                .Where(e => GetUser(e).Driving == true)
                .Select(e => GetUser(e).Email)
                .ToList() : new List<string>();
        }
    }
}
