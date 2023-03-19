using app.Models;
using MongoDB.Driver;

namespace app.Services;

public class PhraseProductsService
{
    private readonly IMongoCollection<PhraseProduct> _products;

    public PhraseProductsService(IMongoCollection<PhraseProduct> products)
    {
        _products = products;
    }

    public async Task<List<PhraseProduct>> GetAsync() =>
        await _products.Find(_ => true).ToListAsync();

    public async Task<PhraseProduct?> GetAsync(string id) =>
        await _products.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(PhraseProduct newPhrase) =>
        await _products.InsertOneAsync(newPhrase);

    public async Task UpdateAsync(string id, PhraseProduct updatedPhrase) =>
        await _products.ReplaceOneAsync(x => x.Id == id, updatedPhrase);

    public async Task RemoveAsync(string id) =>
        await _products.DeleteOneAsync(x => x.Id == id);
}
