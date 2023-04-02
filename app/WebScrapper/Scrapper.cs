using HtmlAgilityPack;
using app.Models;
using MongoDB.Driver;
using System.Threading;
using app.Services;
using NuGet.Packaging;
using Quartz;
using NuGet.DependencyResolver;

namespace DataGeter;

public class Scrapper : IJob
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
            .SelectNodes("//div[contains(@class, 'js_category-list-item')]");
		var i = 0;
        var images = htmlDocument
            .DocumentNode
            .SelectNodes("//div[@class='cat-prod-row__foto']");
        if (images == null) {
			images = htmlDocument
			.DocumentNode
			.SelectNodes("//div[@class='cat-prod-box__picture']");
		}
        
        foreach (var p in products)
        {
            string urlAdress;
            if (images != null)
            {
                if (images[i] != null)
                {
                    if (images[i].ChildNodes[1].ChildNodes[1].GetAttributeValue("src", "No information") == "/content/img/icons/pix-empty.png")
                    {
                        urlAdress = images[i].ChildNodes[1].ChildNodes[1].GetAttributeValue("data-original", "No information");
                    }
                    else
                    {
                        urlAdress = images[i].ChildNodes[1].ChildNodes[1].GetAttributeValue("src", "No information");
                    }
                }
                else
                {
                    urlAdress = "blank";
                }
            }
            else
            {
                urlAdress = "blank";
            }
			
           

            PhraseProduct ProductToAdd = new PhraseProduct
            {
                PhraseCeneoId = p.GetAttributeValue("data-productid", "No information"),
                ProductName = p.GetAttributeValue("data-productname", "No Information"),
                Price = p.GetAttributeValue("data-productminprice", "No Information".Replace(',', '.')),
                CreatedAt = DateTime.Now,
                PhraseName = product,
                ImageUrl = urlAdress
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
        await AddingEveryPhraseProduct(phrases, cancellationToken);
    }

    private async Task AddingEveryPhraseProduct(List<Phrase> phrases, CancellationToken cancellationToken)
    {
        foreach (Phrase phrase in phrases)
        {
            List<PhraseProduct> products = new List<PhraseProduct>();
            products = await GetProductData(phrase.Name);
            await AddProducts(products, cancellationToken);
        }
    }

    public async Task AddProducts(List<PhraseProduct> products, CancellationToken cancellationToken)
    {
        foreach (var product in products)
        {
            await _phraseProductsService.CreateAsync(product, cancellationToken);
        }
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await TrackData(CancellationToken.None);
    }


}
