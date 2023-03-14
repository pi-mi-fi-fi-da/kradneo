using app.Models;
using app.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    public class PhrasesController : Controller
    {
        private readonly PhrasesService _PhrasesService;
       
        private readonly PhraseProductsService _PhraseProductsService;

        public PhrasesController(
            PhrasesService PhrasesService,
            PhraseProductsService PhraseProductsService
            )
        {{}
            _PhrasesService = PhrasesService; 
            _PhraseProductsService = PhraseProductsService;
            }


        /*public PhraseProductsController(PhraseProductsService PhraseProductsService) =>
            _PhraseProductsService = PhraseProductsService;*/

        // GET: PhrasesController
        public async Task<ActionResult<Phrase>> Index()
        {
            var phrases = await _PhrasesService.GetAsync();
            if (phrases is null)
            {
                return NotFound();
            }

            return View(phrases);
        }

        // GET: PhrasesController/Details/5
        public async Task<ActionResult<Phrase>> Details(string id)
        {
            var phrase = await _PhrasesService.GetAsync(id);
            return View(phrase);
        }

        // GET: PhrasesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhrasesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PhrasesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PhrasesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PhrasesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PhrasesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
