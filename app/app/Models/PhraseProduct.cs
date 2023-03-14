using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace app.Models
{
    public class PhraseProduct
    {
        [BsonElement("Id")]
        public string? Id { get; set; }

        [BsonElement("PhraseId")]
        public string PhraseId { get; set; } = null!;

        [BsonElement("ProductName")]
        public string PhraseName { get; set; } = null!;

        [BsonElement("Price")]
        public string Price { get; set; } = null!;

        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; } = null!;

    }
}
