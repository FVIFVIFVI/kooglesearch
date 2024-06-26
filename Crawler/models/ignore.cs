using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ignore.Models
{
    public class Ignore
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

       

        [BsonElement("list_visit_html")]
        public List<string> visited { get; set; } = new List<string>();
    }
}
