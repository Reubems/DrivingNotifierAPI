using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrivingNotifierAPI.Models
{
    public class Request
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        public string IdEntity { get; set; }
        [BsonElement("requestorUsername")]
        public string RequestorUsername { get; set; }
        [BsonElement("requestorEmail")]
        public string RequestorEmail { get; set; }
        [BsonElement("replierEmail")]
        public string ReplierEmail { get; set; }
        [BsonElement("state")]
        public RequestState State { get; set; }
    }

    public enum RequestState
    {   //In BSON they are 0, 1, 2 respectively.
        PENDING, ACCEPTED, DENIED
    }
}
