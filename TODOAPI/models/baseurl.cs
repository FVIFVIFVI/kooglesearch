using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace baseUrlApi.Models;

public class BaseUrl
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string? Name { get; set; }

    [BsonElement("url")]
    public string? Url { get; set; }

    [BsonElement("time")]
    public string? Time { get; set; }

}


