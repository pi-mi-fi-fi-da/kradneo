namespace app.Models;

public class PhraseProductStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string PhraseProductsCollectionName { get; set; } = null!;
}
