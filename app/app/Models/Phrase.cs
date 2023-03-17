using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace app.Models;

public class Phrase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string PhraseName { get; set; } = null!;

}
