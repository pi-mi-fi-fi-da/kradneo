using HtmlAgilityPack;
using app.Models;

namespace DataGeter;

public class Scrapper
{
    public async Task<List<PhraseProduct>> GetProductsData(string productToTrack)
    {
        List<PhraseProduct> result = new List<PhraseProduct>();
        using var httpClient = new HttpClient();

        string ProductToTrack = productToTrack;

        var response = await httpClient.GetAsync($"https://www.ceneo.pl/;szukaj-{ProductToTrack}");

        var content = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='cat-prod-row js_category-list-item js_clickHashData js_man-track-event   ']");
        foreach (var product in products)
        {
            PhraseProduct ProductToAdd = new PhraseProduct();
            ProductToAdd.PhraseId = product.GetAttributeValue("data-productid", "No information");
            ProductToAdd.PhraseName = product.GetAttributeValue("data-productname", "No Information");
            ProductToAdd.Price = product.GetAttributeValue("data-productminprice", "No Information".Replace(',', '.'));
            ProductToAdd.ImageUrl = null;
            result.Add(ProductToAdd);
        }
        return result;
    }
}
