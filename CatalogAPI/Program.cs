using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connectionString, serverVersion));


var app = builder.Build();


//-----------------CONTROLLERS---------------------------

app.MapGet("/categorias/{id:int}", async (int id, MySQLContext db) =>
{
    return await db.Categories.FindAsync(id)
    is Category categoria ? Results.Ok(categoria) : Results.NotFound();
});

app.MapGet("/categorias", async (MySQLContext db) =>
{
    await db.Categories.ToListAsync();
});

app.MapPost("/categorias", async (Category category, MySQLContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{category.Id}", category);
});

app.MapPut("/categorias/{id:int}",async(int id, Category categoria, MySQLContext db) =>
{
    var categoriaDB = await db.Categories.FindAsync(id);
    categoriaDB.Name = categoria.Name;
    categoriaDB.Description = categoria.Description;
    
    if (categoria.Id != id) return Results.BadRequest();
    if (categoriaDB is null) return Results.NotFound();
    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);

   
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();
