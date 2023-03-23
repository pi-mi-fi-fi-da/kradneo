namespace app.Models;

public class PhraseProduct
{
    public string? Id { get; set; }

    public string PhraseId { get; set; } = string.Empty;

    public string? PhraseName { get; set; } = null!;

    public string Price { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }

    public string? ImageUrl { get; set; } = null!;

}
