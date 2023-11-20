using CatalogAPI.Context;
using CatalogAPI.Models;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITokenService>(new TokenService());
builder.Services.AddAuthorization();

//--------------------------ValidarToken-----------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt: Issuer"],
        ValidAudience = builder.Configuration["Jwt: Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt: Key"]))
    };
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connectionString, serverVersion));


var app = builder.Build();

//Endpoint para Login

app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
{
    if(userModel == null) return Results.BadRequest("Invalid Login");

    if (userModel.Name == "gabriel" && userModel.Password == "numsei123")
    {
        var tokenString = tokenService.GenerateToken(app.Configuration["JwtKey"]),
        


    }
});

//-----------------CONTROLLERS CATEGORIAS---------------------------


app.MapGet("/", () => "Catalogo de categorias").ExcludeFromDescription();

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

app.MapPut("/categorias/{id:int}", async (int id, Category categoria, MySQLContext db) =>
{
    var categoriaDB = await db.Categories.FindAsync(id);
    categoriaDB.Name = categoria.Name;
    categoriaDB.Description = categoria.Description;

    if (categoria.Id != id) return Results.BadRequest();
    if (categoriaDB is null) return Results.NotFound();
    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
});

app.MapDelete("/categorias/{id:int}", async (int id, MySQLContext db) =>
{
    var categoria = await db.Categories.FindAsync(id);
    if (categoria is null) return Results.NotFound();


    db.Categories.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
//-----------------CONTROLLERS PRODUTOS---------------------------

app.MapGet("/", () => "Catalogo de Produtos").ExcludeFromDescription();

app.MapGet("/produtos/{id:int}", async (int id, MySQLContext db) =>
{
    return await db.Products.FindAsync(id)
    is Product products ? Results.Ok(products) : Results.NotFound();
});

app.MapGet("/produtos", async (MySQLContext db) =>
{
    await db.Products.ToListAsync();
});

app.MapPost("/produtos", async (Product product, MySQLContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/produtos/{product.Id}", product);
});

app.MapPut("/produtos/{id:int}", async (int id, Product product, MySQLContext db) =>
{
    var productDB = await db.Products.FindAsync(id);
    productDB.Name = productDB.Name;
    productDB.Description = productDB.Description;

    if (productDB.Id != id) return Results.BadRequest();
    if (productDB is null) return Results.NotFound();
    await db.SaveChangesAsync();
    return Results.Ok(productDB);
});

app.MapDelete("/produtos/{id:int}", async (int id, MySQLContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();


    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//-----------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();
