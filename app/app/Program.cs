using app.Models;
using app.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<PhraseStoreDatabaseSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<PhrasesService>();

builder.Services.Configure<PhraseProductStoreDatabaseSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<PhraseProductsService>();

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

app.Run();
