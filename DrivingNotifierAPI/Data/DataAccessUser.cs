using DrivingNotifierAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DrivingNotifierAPI.Utilities;

namespace DrivingNotifierAPI.Data
{
    public class DataAccessUser
    {
        private MongoClient client;
        private IMongoDatabase db;
        private readonly string DB_COLLECTION_NAME_USERS = "Users";
        private readonly string DB_NAME = "DrivingNotifier";
        private IMongoCollection<User> collection;
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
            collection = db.GetCollection<User>(DB_COLLECTION_NAME_USERS);
            IndexKeysDefinition<User> keys = "{ email: 1 }";
            var indexModel = new CreateIndexModel<User>(keys);
            collection.Indexes.CreateOneAsync(indexModel);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        public User GetUser(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);

            return collection.Find(filter).FirstOrDefault();
        }

        public User GetUser(String id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.IdEntity, id);

            return collection.Find(filter).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);

            return collection.Find(filter).FirstOrDefault();
        }

        public void InsertUser(User user)
        {
            Random random = new Random();
            int num = random.Next();
            string hexString = num.ToString("X2");
            string hexValue = DateTime.Now.Ticks.ToString("X2");
            user.IdEntity = hexValue + hexString;
            // user.AccountActivated = false; // TODO: Uncomment
            user.AccountActivated = true; // TODO: Delete Line
            user.TrackingEnabled = false;
            user.Mute = false;
            user.Driving = false;
            user.ResetCode = new string(hexString.ToCharArray());

            collection.InsertOneAsync(user);
           // await VerifyAccountEmail.SendVerifyEmail(user); // TODO: Uncomment

        }

        public async Task UpdateAllUsersDrivingState()
        {
            DateTime tenMinutesAgo = DateTime.Now.AddMinutes(-10);

            var filter = Builders<User>.Filter.Eq(u => u.LastUpdate < tenMinutesAgo, true);
            var update = Builders<User>.Update.Set(s => s.Driving, false);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserPlayerID(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(s => s.PlayerID, user.PlayerID);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task SendResetPasswordEmail(User user)
        {
            await ResetPasswordEmail.SendResetPasswordEmail(user);
        }

        public async Task UpdateUserTrackingEnabledState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(s => s.TrackingEnabled, user.TrackingEnabled);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserActivateAccount(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(s => s.AccountActivated, true);

            await collection.UpdateOneAsync(filter, update);      
        }

        public async Task UpdateUserMuteState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.Set(s => s.Mute, user.Mute);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserDrivingState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
            var update = Builders<User>.Update.
                Set(s => s.Driving, user.Driving).
                Set(s => s.LastUpdate, DateTime.Now);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdatePassword(User user)
        {
            string newCode = new string(DateTime.Now.Ticks.ToString("X").ToCharArray());

            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Email, user.Email),
                Builders<User>.Filter.Eq(u => u.ResetCode, user.ResetCode));

            var update = Builders<User>.Update.
                Set(s => s.Password, user.Password)
                .Set(s => s.ResetCode, newCode);

            await collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);

            await collection.DeleteOneAsync(filter);
        }

        public async Task DeleteUser(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.IdEntity, userId);

            await collection.DeleteOneAsync(filter);
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
                "\n\t\"contents\" : { \"en\" : \" " + user.FullName + " is driving now!\" }," +
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
                await collection.UpdateOneAsync(filter, update);
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

                await collection.UpdateOneAsync(filter, update);
            }
        }

        public List<string> GetContactsList(string email)
        {
            User requestor = GetUserByEmail(email);

            return requestor.Contacts != null ? requestor.Contacts.
                Select(e => GetUser(e).Email).ToList() : new List<string>();
        }

        public List<string> GetContactsDrivingList(string email)
        {
            User requestor = GetUserByEmail(email);
            DateTime tenMinutesAgo = DateTime.Now.AddMinutes(-10);

            return requestor.Contacts != null ? requestor.Contacts
                .Where(e => GetUser(e).TrackingEnabled == true)
                .Where(e => GetUser(e).Driving == true)
                .Where(e => GetUser(e).LastUpdate > tenMinutesAgo)
                .Select(e => GetUser(e).Email)
                .ToList() : new List<string>();
        }
    }
}
