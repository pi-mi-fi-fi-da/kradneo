using app.Models;
using MongoDB.Driver;

namespace app.Services;

public class PhrasesService
{
    private readonly IMongoCollection<Phrase> _PhrasesCollection;

    public PhrasesService(IMongoCollection<Phrase> PhrasesCollection)
    {
        _PhrasesCollection = PhrasesCollection;
    }

    public async Task<List<Phrase>> GetAllAsync(CancellationToken cancellationToken) =>
        await _PhrasesCollection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Phrase?> GetOneAsync(string id, CancellationToken cancellationToken) =>
        await _PhrasesCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task CreateAsync(Phrase newPhrase, CancellationToken cancellationToken) =>
        await _PhrasesCollection.InsertOneAsync(newPhrase, cancellationToken);

    public async Task RemoveAsync(string id, CancellationToken cancellationToken) =>
        await _PhrasesCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
}
