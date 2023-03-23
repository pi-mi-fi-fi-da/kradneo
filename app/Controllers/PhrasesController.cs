using app.Models;
using app.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers;

public class PhrasesController : Controller
{
    private readonly PhrasesService _PhrasesService;

    private readonly PhraseProductsService _PhraseProductsService;

    public PhrasesController(
        PhrasesService PhrasesService,
        PhraseProductsService PhraseProductsService
        )
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
    public async Task<ActionResult<Phrase>> Details(string id, CancellationToken cancellationToken)
    {
        var phrase = await _PhrasesService.GetOneAsync(id, cancellationToken);
        return View(phrase);
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
        return RedirectToAction("Index");
    }

    // GET: PhrasesController/Edit/5
    public async Task<ActionResult<Phrase>> Edit(string id, CancellationToken cancellationToken)
    {
        var phrase = await _PhrasesService.GetOneAsync(id, cancellationToken);
        return View(phrase);
    }

    // POST: PhrasesController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Phrase>> Edit(string id, Phrase newPhrase, CancellationToken cancellationToken)
    {
        await _PhrasesService.UpdateAsync(id, newPhrase, cancellationToken);
        return RedirectToAction("Index");
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
}
