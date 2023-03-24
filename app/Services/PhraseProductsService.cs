using app.Models;
using MongoDB.Driver;
using System.Threading;

namespace app.Services;

public class PhraseProductsService
{
    private readonly IMongoCollection<PhraseProduct> _PhrasesProductsCollection;

    public PhraseProductsService(IMongoCollection<PhraseProduct> PhrasesProductsCollection)
    {
        _PhrasesProductsCollection = PhrasesProductsCollection;
    }

    public async Task<List<PhraseProduct>> GetAllAsync(CancellationToken cancellationToken) =>
        await _PhrasesProductsCollection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<List<PhraseProduct>> GetAllByPhraseNameAsync(string phraseName, CancellationToken cancellationToken) =>
       await _PhrasesProductsCollection.Find(x => x.PhraseName == phraseName).ToListAsync(cancellationToken);

    public async Task<PhraseProduct?> GetOneAsync(string id, CancellationToken cancellationToken) =>
        await _PhrasesProductsCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task CreateAsync(PhraseProduct newPhrase, CancellationToken cancellationToken) =>
        await _PhrasesProductsCollection.InsertOneAsync(newPhrase, cancellationToken);

    public async Task RemoveAsync(string id, CancellationToken cancellationToken) =>
        await _PhrasesProductsCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
}
