

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace usersApi.Models;

public class Users
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("user name")]
    public string? User_name { get; set; }

    [BsonElement("password")]
    public string? Password { get; set; }

}

