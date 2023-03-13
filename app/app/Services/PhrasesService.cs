using app.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace app.Services
{
    public class PhrasesService
    {
        private readonly IMongoCollection<Phrase> _PhrasesCollection;

        public PhrasesService(
            IOptions<PhraseStoreDatabaseSettings> PhraseStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                PhraseStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                PhraseStoreDatabaseSettings.Value.DatabaseName);

            _PhrasesCollection = mongoDatabase.GetCollection<Phrase>(
                PhraseStoreDatabaseSettings.Value.PhrasesCollectionName);
        }

        public async Task<List<Phrase>> GetAsync() =>
            await _PhrasesCollection.Find(_ => true).ToListAsync();

        public async Task<Phrase?> GetAsync(string id) =>
            await _PhrasesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Phrase newPhrase) =>
            await _PhrasesCollection.InsertOneAsync(newPhrase);

        public async Task UpdateAsync(string id, Phrase updatedPhrase) =>
            await _PhrasesCollection.ReplaceOneAsync(x => x.Id == id, updatedPhrase);

        public async Task RemoveAsync(string id) =>
            await _PhrasesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
