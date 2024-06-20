using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace spamApi.Models;

public class Spam
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

}

