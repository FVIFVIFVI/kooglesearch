using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ignore.Models
{
    public class Ignore
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

       

        [BsonElement("HashSet")]
        public HashSet<string> HashSet { get; set; } = new HashSet<string>();
    }
}
