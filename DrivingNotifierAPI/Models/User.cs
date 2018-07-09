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
        [BsonElement("Phone")]
        public string Phone { get; set; }
        [BsonElement("PlayerID")]
        public string PlayerID { get; set; }
        [BsonElement("TrackingEnabled")]
        public bool TrackingEnabled { get; set; }
        [BsonElement("Mute")]
        public bool Mute { get; set; }
        [BsonElement("Driving")]
        public bool Driving { get; set; }
        [BsonElement("Contacts")]
        [BsonRepresentation(BsonType.String)]
        public List<ObjectId> Contacts { get; set; }
    }
}
