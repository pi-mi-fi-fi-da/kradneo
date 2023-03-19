using HtmlAgilityPack;
using app.Models;

namespace DataGeter;

public class Scrapper
{
    public async Task<List<PhraseProduct>> GetProductsData(string productToTrack)
    {
        List<PhraseProduct> result = new List<PhraseProduct>();
        using var httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://www.ceneo.pl/")
        };

        string ProductToTrack = productToTrack;

        var response = await httpClient.GetAsync($";szukaj-{ProductToTrack}");

        var content = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='cat-prod-row js_category-list-item js_clickHashData js_man-track-event   ']");

        foreach (var product in products)
        {
            PhraseProduct ProductToAdd = new PhraseProduct
            {
                PhraseId = product.GetAttributeValue("data-productid", "No information"),
                PhraseName = product.GetAttributeValue("data-productname", "No Information"),
                Price = product.GetAttributeValue("data-productminprice", "No Information".Replace(',', '.')),
                ImageUrl = null
            };
            result.Add(ProductToAdd);
        }

        return result;
    }
}
