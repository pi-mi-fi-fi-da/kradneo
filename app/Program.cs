using app.Models;
using app.quartz;
using app.Services;
using DataGeter;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//MongoDB
var pack = new ConventionPack
{
    new CamelCaseElementNameConvention(),
    new StringIdStoredAsObjectIdConvention()
};

ConventionRegistry.Register("kradneo_convenction", pack, _ => true);
MongoUrl url = new MongoUrl(builder.Configuration.GetConnectionString("MongoDB"));
MongoClientSettings settings = MongoClientSettings.FromUrl(url);
MongoClient client = new MongoClient(settings);
IMongoDatabase database = client.GetDatabase(url.DatabaseName);

//Colections
IMongoCollection<Phrase> phrases = database.GetCollection<Phrase>("phrases");
IMongoCollection<PhraseProduct> products = database.GetCollection<PhraseProduct>("products");

builder.Services.AddSingleton(phrases);
builder.Services.AddSingleton(products);

//Services
builder.Services.AddScoped<IPhrasesService, PhrasesService>();
builder.Services.AddScoped<IPhrasesProductService, PhraseProductsService>();

//quartz

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    var jobKey = new JobKey("Scrapper");
    q.AddJob<Scrapper>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("DemoJob-trigger")
        .WithCronSchedule("0 0/5 * * * ?"));

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Phrases}/{action=Index}/{id?}");

//app.Lifetime.ApplicationStarted.Register(async () =>
//{
//    //var phrases = app.Services.GetRequiredService<IMongoCollection<Phrase>>();
//    //await phrases.InsertOneAsync(new Phrase { Name = $"fraza_{DateTime.UtcNow}" });

//    //var products = app.Services.GetRequiredService<IMongoCollection<PhraseProduct>>();
//    //await products.InsertOneAsync(new PhraseProduct { PhraseName = $"fraza_{DateTime.UtcNow}" });
//});
//Scrapper scrapper = new Scrapper(new PhraseProductsService(products), new PhrasesService(phrases));
//await scrapper.TrackData(CancellationToken.None);

app.Run();


