using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrivingNotifierAPI.Models
{
    public class Request
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("requestorPhone")]
        public string RequestorPhone { get; set; }
        [BsonElement("replierPhone")]
        public string ReplierPhone { get; set; }
        [BsonElement("state")]
        public RequestState State { get; set; }
    }

    public enum RequestState
    {   //In BSON they are 0, 1, 2 respectively.
        PENDING, ACCEPTED, DENIED
    }
}
