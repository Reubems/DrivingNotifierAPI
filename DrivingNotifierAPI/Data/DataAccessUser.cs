using DrivingNotifierAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
            return  db.GetCollection<User>("Users").Find(filter).FirstOrDefault();
        }

        public  User GetUserByPhone(string phone)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Phone, phone);
            return db.GetCollection<User>("Users").Find(filter).FirstOrDefault();
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

        public async Task UpdateUserTrackingEnabledState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Phone, user.Phone);
            var update = Builders<User>.Update.Set(s => s.TrackingEnabled, user.TrackingEnabled);
            await db.GetCollection<User>("Users").UpdateOneAsync(filter, update);
        }

        public async Task UpdateUserMuteState(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Phone, user.Phone);
            var update = Builders<User>.Update.Set(s => s.Mute, user.Mute);
            await db.GetCollection<User>("Users").UpdateOneAsync(filter, update);
        }


        public async Task DeleteUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Phone, user.Phone);
            await db.GetCollection<User>("Users").DeleteOneAsync(filter);
        }

        //Push notification

        public void PushNotification(string phone) 
        {
            User user = GetUserByPhone(phone);

            var contacts = user.Contacts;
            StringBuilder sb = new StringBuilder();

            foreach (ObjectId id in contacts ){
                User fetched = GetUser(id);
                string playerID = fetched.PlayerID;
                string playerIDformatted = string.Format("\"{0}\",", playerID);
                sb.Append(playerIDformatted);
            }
            sb.Remove(sb.Length - 1, 1); //To remove the last ",".

            var client = new RestClient("https://onesignal.com/api/v1/notifications");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic ZjMyN2JiMDctNDI4ZC00OWI1LWEyZTAtMjBjYTE5MjYzOTk4");
            request.AddParameter("undefined", "{\n\t\"app_id\" : \"29d6b657-f182-41e7-85cd-1807d337fdfc\" ,\n\t\"contents\" : { \"en\" : \"I am driving now!\" },\n\t\"include_player_ids\" : ["+ sb +"]\n}", ParameterType.RequestBody);
            client.ExecuteAsync(request, (response) =>
            {
               Console.WriteLine(response.StatusCode); //TODO Change for Application Insights
            });
        }

        //Update Contacts List
        public async Task UpdateUserContactList(string phoneRequestor, string phoneReplier)
        {
            User requestor = GetUserByPhone(phoneRequestor);
            User replier = GetUserByPhone(phoneReplier);
            if(requestor != null && replier != null)
            {
                var contacts = new List<ObjectId>();
                if (replier.Contacts != null)
                {
                     contacts = replier.Contacts;
                }

                contacts.Add(requestor.Id); //We add the ObjectId

                var filter = Builders<User>.Filter.Eq(u => u.Phone, replier.Phone);
                var update = Builders<User>.Update.Set(s => s.Contacts, contacts);
                await db.GetCollection<User>("Users").UpdateOneAsync(filter, update);
            }
        }
    }
}
