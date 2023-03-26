using app.Models;
using app.Services;
using DataGeter;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

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
Scrapper scrapper = new Scrapper(new PhraseProductsService(products), new PhrasesService(phrases));
await scrapper.TrackData(CancellationToken.None);
app.Run();


