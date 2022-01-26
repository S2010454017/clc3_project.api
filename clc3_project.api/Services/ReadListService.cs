using CLC3_Project.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CLC3_Project.Services
{
    public class ReadListService
    {
        private readonly IMongoCollection<ReadingList> _listCollection;

        public ReadListService(IOptions<ReadListDatabaseSettings> readListDBSettings)
        {
            var mongoClient = new MongoClient(readListDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(readListDBSettings.Value.DatabaseName);
            _listCollection = mongoDatabase.GetCollection<ReadingList>(readListDBSettings.Value.BooksCollectionName);
        }

        public async Task<List<ReadingList>> GetAsync() =>
           await _listCollection.Find(_ => true).ToListAsync();

        public async Task<List<ReadingList>> GetListForOwnerAsync(string owner) =>
          await _listCollection.Find(x => x.Owner == owner).ToListAsync();

        public async Task<ReadingList?> GetAsync(string name, string owner) =>
            await _listCollection.Find(x => x.Name == name && x.Owner == owner).FirstOrDefaultAsync();

        public async Task CreateAsync(ReadingList newList) =>
            await _listCollection.InsertOneAsync(newList);

        public async Task UpdateAsync(ReadingList updateList) =>
            await _listCollection.ReplaceOneAsync(x =>x.Id == updateList.Id, updateList);

        public async Task RemoveAsync(string id) =>
            await _listCollection.DeleteOneAsync(x => x.Id == id);
    }
}
