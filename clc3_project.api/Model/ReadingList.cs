using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CLC3_Project.Model
{
    /// <summary>
    /// Represents one reading list of a user.
    /// </summary>
    public class ReadingList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //user who owns this list
        public string Owner { get; set; } = null!;

        //name of the list
        public string Name { get; set; } = null!;

        //all books in this list
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
