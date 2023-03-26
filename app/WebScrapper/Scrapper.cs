using HtmlAgilityPack;
using app.Models;
using MongoDB.Driver;
using System.Threading;
using app.Services;
using Quartz;

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

        var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='cat-prod-row js_category-list-item js_clickHashData js_man-track-event   ']");

        foreach (var p in products)
        {
            PhraseProduct ProductToAdd = new PhraseProduct
            {
                PhraseCeneoId = p.GetAttributeValue("data-productid", "No information"),
                PhraseName = p.GetAttributeValue("data-productname", "No Information"),
                Price = p.GetAttributeValue("data-productminprice", "No Information".Replace(',', '.')),
                CreatedAt = DateTime.Now,
                ImageUrl = null
            };
            result.Add(ProductToAdd);
        }

        return result;
    }
    public async Task TrackData(CancellationToken cancellationToken)
    {
        List<Phrase> phrases = new List<Phrase>();
        phrases = await _phrasesService.GetAllAsync(cancellationToken);
        foreach(Phrase phrase in phrases)
        {
            List<PhraseProduct> products = new List<PhraseProduct>();
            List<PhraseProduct> productsAtDatabaseCeneoId = new List<PhraseProduct>();
            products = await GetProductData(phrase.Name);
            productsAtDatabaseCeneoId = await _phraseProductsService.GetAllAsync(cancellationToken);
            foreach(var product in products)
            {
                if(!productsAtDatabaseCeneoId.Exists(x => x.PhraseCeneoId == product.PhraseCeneoId))
                {
                    await _phraseProductsService.CreateAsync(product, cancellationToken);
                }
                else
                {
                    PhraseProduct productToAdd = new PhraseProduct() //produkt bez podstawowych danych
                    {
                        PhraseCeneoId = product.PhraseCeneoId,
                        Price = product.Price,
                        CreatedAt = product.CreatedAt,
                        PhraseName = null,
                        ImageUrl = null

                    };
                    await _phraseProductsService.CreateAsync(productToAdd, cancellationToken);
                }
            }
        }
    }
    
}
