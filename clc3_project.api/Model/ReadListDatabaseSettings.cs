namespace CLC3_Project.Model
{
    /// <summary>
    /// Handles the necessary Configuration for database access.
    /// Responsible  for the book collection
    /// </summary>
    public class ReadListDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BooksCollectionName { get; set; } = null!;
    }
}
