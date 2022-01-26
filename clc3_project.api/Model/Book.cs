using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CLC3_Project.Model
{
    /// <summary>
    /// Class for representing a book. 
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? ISBN { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; } = null!;

        // all categories according to openlibrary
        public HashSet<string> Category { get; set; } = new HashSet<string>();

        // contains all authors who helped write this book
        public HashSet<string> Authors { get; set; } = new HashSet<string>();

        // stores the url for a cover picutre
        public string? Cover { get; set; }
    }
}
