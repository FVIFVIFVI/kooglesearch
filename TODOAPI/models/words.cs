
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wordsApi.Models;
public class Words
{

    // [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("dict")]
    public List<Dict>? Dict { get; set; }

}

public class Dict
{
    [BsonElement("url")]
    public string? Url { get; set; }

    [BsonElement("count")]
    public int Count { get; set; }
}




