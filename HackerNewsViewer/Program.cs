using HackerNewsViewer.Clients;
using HackerNewsViewer.Services; // Will create this next
using Microsoft.Extensions.Caching.Memory; // For caching

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient for HackerNewsApiClient
builder.Services.AddHttpClient<IHackerNewsApiClient, HackerNewsApiClient>();
// Add Memory Cache
builder.Services.AddMemoryCache();
// Register your Story Service
builder.Services.AddScoped<IStoryService, StoryService>();

// Configure CORS (important for Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Angular dev server URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
            // For production, replace "http://localhost:4200" with your deployed Angular URL
            // Example: .WithOrigins("https://your-angular-app.azurewebsites.net")
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp"); // Enable CORS middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
