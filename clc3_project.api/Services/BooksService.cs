using CLC3_Project.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace CLC3_Project.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly HttpClient client;
        private readonly string urlFetchBook;

        public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings,
                            HttpClient client, IConfiguration config)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
            this.client = client;
            urlFetchBook = config.GetSection("AZURE_FUNCTION").GetSection("URL").Value;
        }

        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id, bool fetch = false)
        {
            var b = await _booksCollection.Find(x => x.ISBN == id).FirstOrDefaultAsync();


            if (b == default && fetch)
            {
                var resp = await client.GetAsync(string.Format(urlFetchBook, id));
                if (resp.IsSuccessStatusCode)
                {
                    b = await resp.Content.ReadFromJsonAsync<Book>();
                    if (b is not null)
                    {
                        await _booksCollection.InsertOneAsync(b);
                    }
                }
            }
            return b;
        }

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.ISBN == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
