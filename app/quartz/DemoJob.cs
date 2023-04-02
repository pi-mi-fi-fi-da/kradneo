using app.Services;
using DataGeter;
using Quartz;

namespace app.quartz;

public class DemoJob : IJob
{
    private readonly IPhrasesService _PhrasesService;

    private readonly IPhrasesProductService _PhraseProductsService;
    private readonly Scrapper _scrapper;

    public DemoJob(IPhrasesService PhrasesService, IPhrasesProductService PhraseProductsService, Scrapper scrapper)
    {
        _PhrasesService = PhrasesService;
        _PhraseProductsService = PhraseProductsService;
        _scrapper = scrapper;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            //var phrases = await _PhrasesService.GetAllAsync(CancellationToken.None);
            //Scrapper scrapper = new Scrapper(new PhraseProductsService(products), new PhrasesService(phrases));
            await _scrapper.TrackData(CancellationToken.None);
        }
        catch
        {
            throw;
        }

        //return Task.CompletedTask;
    }
}
