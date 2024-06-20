using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace urlsApi.Models;

public class Urls
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("time")]
    public string? Time { get; set; }

    [BsonElement("children")]
    public List<Dict>? Children { get; set; }
}

public class Dict
{
    [BsonElement("url")]
    public string? Url { get; set; }

    [BsonElement("count")]
    public int? Count { get; set; }
}

  