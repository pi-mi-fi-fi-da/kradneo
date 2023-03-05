using HtmlAgilityPack;
namespace DataGeter;

public class CeneoDataGeter
{
	private readonly string ProductToTrack;
	public CeneoDataGeter(string productToTrack)
	{
		ProductToTrack = productToTrack;
	}
	public async Task<List<Product>> GetProductsData()
	{
        List<Product> result = new List<Product>();
        using var httpClient = new HttpClient();

        var response = await httpClient.GetAsync($"https://www.ceneo.pl/;szukaj-{ProductToTrack}");
        
        var content = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='cat-prod-row js_category-list-item js_clickHashData js_man-track-event   ']");
        foreach (var product in products)
        {
            Product ProductToAdd = new Product();
            ProductToAdd.Price = product.GetAttributeValue("data-productminprice", "No Information".Replace(',', '.'));
            ProductToAdd.Name = product.GetAttributeValue("data-productname", "No Information");
            result.Add(ProductToAdd);
        }
        return result;
    }

}
