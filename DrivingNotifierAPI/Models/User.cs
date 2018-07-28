﻿using MongoDB.Bson;
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
        //TODO Add Password and full name.
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("playerID")]
        public string PlayerID { get; set; }
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
