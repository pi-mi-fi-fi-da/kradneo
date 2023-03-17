namespace app.Models;

public class PhraseStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string PhrasesCollectionName { get; set; } = null!;
}
