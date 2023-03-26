using app.Models;

namespace app.Services;

public interface IPhrasesProductService
{
    public  Task<List<PhraseProduct>> GetAllAsync(CancellationToken cancellationToken);
    public  Task<List<PhraseProduct>> GetAllByPhraseNameAsync(string phraseName, CancellationToken cancellationToken);
    public  Task<PhraseProduct?> GetOneAsync(string id, CancellationToken cancellationToken);
    public Task CreateAsync(PhraseProduct newPhrase, CancellationToken cancellationToken);
    public  Task RemoveAsync(string id, CancellationToken cancellationToken);
}
