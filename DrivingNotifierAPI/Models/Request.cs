using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingNotifierAPI.Models
{
    public class Request
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("RequestorPhone")]
        public string RequestorPhone { get; set; }
        [BsonElement("ReplierPhone")]
        public string ReplierPhone { get; set; }
        [BsonElement("State")]
        public RequestState State { get; set; }

    }

    public enum RequestState
    {
        NEW, PENDING, ACCEPTED, DENIED
    }
}
