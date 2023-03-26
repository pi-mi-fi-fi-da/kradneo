using HtmlAgilityPack;
using app.Models;
using MongoDB.Driver;
using System.Threading;
using app.Services;
using NuGet.Packaging;
//using Quartz;

namespace DataGeter;

public class Scrapper
{
    private readonly IPhrasesProductService _phraseProductsService;
    private readonly IPhrasesService _phrasesService;
    public Scrapper(IPhrasesProductService phraseProductsService, IPhrasesService phrasesService)
    {
        _phraseProductsService = phraseProductsService;
        _phrasesService = phrasesService;
    }
    public async Task<List<PhraseProduct>> GetProductData(string product)
    {
        List<PhraseProduct> result = new List<PhraseProduct>();
        using var httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://www.ceneo.pl/")
        };

        string Product = product;

        var response = await httpClient.GetAsync($";szukaj-{Product}");

        var content = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        var products = htmlDocument
            .DocumentNode
            .SelectNodes("//div[@class='cat-prod-row js_category-list-item js_clickHashData js_man-track-event   js_redirectorLinkData']");
        var products2 = htmlDocument.
            DocumentNode
            .SelectNodes("//div[@class='cat-prod-row js_category-list-item js_clickHashData js_man-track-event   ']");
        products.AddRange(products2);
        var images = htmlDocument
            .DocumentNode
            .SelectNodes("//div[@class='cat-prod-row__foto']");
        //foreach (var image in images)
        //{
        //    var x = image.ChildNodes[1].ChildNodes[1].GetAttributeValue("src", "No information");
        //}
        var i = 0;
        foreach (var p in products)
        {
            PhraseProduct ProductToAdd = new PhraseProduct
            {
                PhraseCeneoId = p.GetAttributeValue("data-productid", "No information"),
                ProductName = p.GetAttributeValue("data-productname", "No Information"),
                Price = p.GetAttributeValue("data-productminprice", "No Information".Replace(',', '.')),
                CreatedAt = DateTime.Now,
                PhraseName = product,
                ImageUrl = images[i].ChildNodes[1].ChildNodes[1].GetAttributeValue("src", "No information")
            };
            result.Add(ProductToAdd);
            i++;
        }

        return result;
    }
    public async Task TrackData(CancellationToken cancellationToken)
    {
        List<Phrase> phrases = new List<Phrase>();
        phrases = await _phrasesService.GetAllAsync(cancellationToken);
        foreach (Phrase phrase in phrases)
        {
            List<PhraseProduct> products = new List<PhraseProduct>();
            products = await GetProductData(phrase.Name);
            foreach (var product in products)
            {
                await _phraseProductsService.CreateAsync(product, cancellationToken);
            }
        }
    }

}
