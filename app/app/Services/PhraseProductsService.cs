using app.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace app.Services;

public class PhraseProductsService
{
    private readonly IMongoCollection<Phrase> _PhraseProductsCollection;

    public PhraseProductsService(
        IOptions<PhraseProductStoreDatabaseSettings> PhraseProductstoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            PhraseProductstoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            PhraseProductstoreDatabaseSettings.Value.DatabaseName);

        _PhraseProductsCollection = mongoDatabase.GetCollection<Phrase>(
            PhraseProductstoreDatabaseSettings.Value.PhraseProductsCollectionName);
    }

    public async Task<List<Phrase>> GetAsync() =>
        await _PhraseProductsCollection.Find(_ => true).ToListAsync();

    public async Task<Phrase?> GetAsync(string id) =>
        await _PhraseProductsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Phrase newPhrase) =>
        await _PhraseProductsCollection.InsertOneAsync(newPhrase);

    public async Task UpdateAsync(string id, Phrase updatedPhrase) =>
        await _PhraseProductsCollection.ReplaceOneAsync(x => x.Id == id, updatedPhrase);

    public async Task RemoveAsync(string id) =>
        await _PhraseProductsCollection.DeleteOneAsync(x => x.Id == id);
}
