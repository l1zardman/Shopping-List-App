using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Api.Configuration;
using ShoppingListApp.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = 
            ReferenceHandler.IgnoreCycles;
    });;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShoppingListDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ShoppingListDb")));

builder.Services.AddScoped<IShoppingListRepository, ShoppingListShoppingListRepository>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("http://localhost:5000") // Blazor app URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ShoppingListDbContext>();
    
    dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();
}

SeedData.EnsurePopulated(app);

app.UseCors("AllowBlazorApp");

app.Run();