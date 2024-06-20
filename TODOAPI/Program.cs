using KoogleDatabaseSettingsApi.Models;
using BaseUrlApi.Services;
using SpamApi.Services;
using UrlsApi.Services;
using UsersApi.Services;
using WordsApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KoogleDatabaseSettings>(builder.Configuration.GetSection("KoogleDatabase"));

builder.Services.AddSingleton<KoogleDatabaseSettings>();
builder.Services.AddSingleton<BaseUrlService>();
builder.Services.AddSingleton<SpamService>();
builder.Services.AddSingleton<UrlsService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<WordsService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "http://www.contoso.com")
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(MyAllowSpecificOrigins);

app.UseStaticFiles();

app.UseAuthorization();

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();



