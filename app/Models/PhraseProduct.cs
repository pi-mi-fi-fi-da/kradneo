namespace app.Models;

public class PhraseProduct
{
    public string? Id { get; set; }
    public string PhraseName { get; set; } = string.Empty;
    public string PhraseCeneoId { get; set; } = string.Empty;
	public string ProductName { get; set; } = string.Empty;
	public string Price { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }

}
