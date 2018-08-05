using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingNotifierAPI.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        [BsonElement("idEntity")]
        public string IdEntity { get; set; }
        [BsonIgnoreIfNull]
        [BsonElement("resetCode")]
        public string ResetCode { get; set; }
        [BsonElement("fullName")]
        public string FullName { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("playerID")]
        public string PlayerID { get; set; }
        [BsonElement("accountActivated")]
        public bool AccountActivated { get; set; }
        [BsonElement("trackingEnabled")]
        public bool TrackingEnabled { get; set; }
        [BsonElement("mute")]
        public bool Mute { get; set; }
        [BsonElement("driving")]
        public bool Driving { get; set; }
        [BsonElement("lastUpdate")]
        public DateTime LastUpdate { get; set; }
        [BsonElement("contacts")]
        [BsonRepresentation(BsonType.String)]
        public List<ObjectId> Contacts { get; set; }
    }
}
