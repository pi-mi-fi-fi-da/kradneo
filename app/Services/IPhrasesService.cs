using app.Models;

namespace app.Services;

public interface IPhrasesService
{
    public Task<List<Phrase>> GetAllAsync(CancellationToken cancellationToken);
    public Task<Phrase?> GetOneAsync(string id, CancellationToken cancellationToken);
    public Task CreateAsync(Phrase newPhrase, CancellationToken cancellationToken);
    public Task RemoveAsync(string id, CancellationToken cancellationToken);
}
