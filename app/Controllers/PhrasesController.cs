using app.Models;
using app.Services;
using DataGeter;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

namespace app.Controllers;

public class PhrasesController : Controller
{
    private readonly IPhrasesService _PhrasesService;

    private readonly IPhrasesProductService _PhraseProductsService;

    public PhrasesController(IPhrasesService PhrasesService, IPhrasesProductService PhraseProductsService)
    {
        _PhrasesService = PhrasesService;
        _PhraseProductsService = PhraseProductsService;
    }

    // GET: PhrasesController
    public async Task<ActionResult<List<Phrase>>> Index(CancellationToken cancellationToken)
    {
        var phrases = await _PhrasesService.GetAllAsync(cancellationToken);

        return phrases is null 
            ? NotFound() 
            : View(phrases);
    }

    // GET: PhrasesController/Details/5
    public async Task<ActionResult<PhraseDetail>> Details(string id, CancellationToken cancellationToken)
    {
        var phrase = await _PhrasesService.GetOneAsync(id, cancellationToken);
        if (phrase is null) return NotFound();
        var phraseProducts = await _PhraseProductsService.GetAllByPhraseNameAsync(phrase.Name, cancellationToken);
        var phraseDetails = new PhraseDetail(phrase, phraseProducts);
        return View(phraseDetails);
    }

    // GET: PhrasesController/Create
    public ActionResult Create(CancellationToken cancellationToken)
    {
        return View();
    }

    // POST: PhrasesController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Phrase>> Create(Phrase newPhrase, CancellationToken cancellationToken)
    {
        await _PhrasesService.CreateAsync(newPhrase, cancellationToken);
        Scrapper scrapper = new Scrapper(_PhraseProductsService, _PhrasesService);
        var products = await scrapper.GetProductData(newPhrase.Name);
        await scrapper.AddProducts(products, CancellationToken.None);
        return RedirectToAction(nameof(Index));
    }

    // GET: PhrasesController/Delete/5
    public async Task<ActionResult<Phrase>> Delete(string id, CancellationToken cancellationToken)
    {
        var phrase = await _PhrasesService.GetOneAsync(id, cancellationToken);
        if (phrase is null)
        {
            return NotFound();
        }
        return View(phrase);
    }

    // POST: PhrasesController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Phrase>> Delete(Phrase newPhrase, CancellationToken cancellationToken)
    {
        if (newPhrase.Id is null)
        {
            return View();
        }
        else
        {
            await _PhrasesService.RemoveAsync(newPhrase.Id, cancellationToken);
            return RedirectToAction("Index");
        }
    }
    public IActionResult Privacy()
    {
        return View();
    }
}
