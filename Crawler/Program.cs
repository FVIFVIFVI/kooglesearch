using KoogleDatabaseSettingsApi.Models;
using BaseUrlApi.Services;
using SpamApi.Services;
using UrlsApi.Services;
using UsersApi.Services;
using WordsApi.Services;
using playCrawler;
using upserver; // הוסף את ה-namespace של Upserver
using pageRank; // הוסף את ה-namespace של PageRank
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.Configure<KoogleDatabaseSettings>(builder.Configuration.GetSection("KoogleDatabase"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<KoogleDatabaseSettings>>().Value);
builder.Services.AddSingleton<BaseUrlService>();
builder.Services.AddSingleton<SpamService>();
builder.Services.AddSingleton<UrlsService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<WordsService>();
builder.Services.AddSingleton<PlayCrawler>();
builder.Services.AddSingleton<Upserver>(); // הוסף את השורה הזו
builder.Services.AddSingleton<PageRank>(); // הוסף את השורה הזו

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5016", "http://localhost:3000") // Allow access from both 5016 and 3000
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Create a scope for services
using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

// Instantiate and use PlayCrawler
var playCrawler = serviceProvider.GetRequiredService<PlayCrawler>();
//await playCrawler.plaing();

app.Run();
