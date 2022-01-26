using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CLC3_Project.Model
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? ISBN { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; } = null!;

        public HashSet<string> Category { get; set; } = new HashSet<string>();

        public HashSet<string> Authors { get; set; } = new HashSet<string>();

        public string? Cover { get; set; }
    }
}
