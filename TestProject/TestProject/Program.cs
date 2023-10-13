using Microsoft.EntityFrameworkCore;
using TestProject.Domain.Interfaces;
using TestProject.Domain.Services;
using TestPtoject.DAL;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Додавання сервісів в контейнер.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

var rateLimitOptions = new RateLimitOptions
{
    GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "/api/Dogs/get-dogs",
            Limit = 10,
            Period = "s"
        },
        new RateLimitRule
        {
            Endpoint = "/api/Dogs/create-dog",
            Limit = 10,
            Period = "s"
        },
        new RateLimitRule
        {
            Endpoint = "/api/Dogs/ping",
            Limit = 10,
            Period = "s"
        }
    }
};

builder.Services.Configure<RateLimitOptions>(options =>
{
    options.GeneralRules = rateLimitOptions.GeneralRules;
});

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

builder.Services.AddScoped<IDogsService, DogsService>();

builder.Services.AddDbContext<DogContext>(options =>
    options.UseSqlServer("Server=.;Database=Dogs;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
