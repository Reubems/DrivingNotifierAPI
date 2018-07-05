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

        //Añadir atributo que sea un array de ObjectId con los contactos aceptados por el usuario.  //Swagger does not get it
        //[BsonElement("Contacts")]
        //public BsonArray Contacts { get; set; }

        [BsonElement("Contacts")]
        [BsonRepresentation(BsonType.String)]
        public List<ObjectId> Contacts { get; set; }
    }
}
