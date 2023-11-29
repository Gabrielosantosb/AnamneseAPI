using CatalogAPI.Context;
using CatalogAPI.Models;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() 
                { 
                    Name = "Authorization", 
                    Type = SecuritySchemeType.ApiKey, 
                    Scheme = "Bearer", 
                    BearerFormat = "JWT", 
                    In = ParameterLocation.Header, 
                   // Description = "JWT Authorization header using the Bearer scheme.
                   //\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.
                   // \r\n\r\nExample: \"Bearer 12345abcdef\"", 
                }); 
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                { 
                    { 
                          new OpenApiSecurityScheme 
                          { 
                              Reference = new OpenApiReference 
                              { 
                                  Type = ReferenceType.SecurityScheme, 
                                  Id = "Bearer" 
                              } 
                          }, 
                         new string[] {} 
                    } 
                }); 
   });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<ITokenService>(new TokenService());
//--------------------------ValidarToken-----------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connectionString, serverVersion));



var app = builder.Build();

//-------------------Endpoint para Login---------------------------

app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
{
    if (userModel == null) return Results.BadRequest("Login Inválido");
    if (userModel.UserName == "gabriel" && userModel.Password == "1234")
    {
        var tokenString = tokenService.GenerateToken(
            app.Configuration["Jwt:Key"],
            app.Configuration["Jwt:Issuer"],
            app.Configuration["Jwt:Audience"],
            userModel
            );
        return Results.Ok(new { token = tokenString });
    }
    else return Results.BadRequest("Login Inválido");

}).Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status200OK)
.WithName("Login")
.WithTags("Autentificacao");



//-----------------CONTROLLERS CATEGORIAS---------------------------


app.MapGet("/", () => "Catalogo de categorias").ExcludeFromDescription().WithTags("Categorias");

app.MapGet("/categorias/{id:int}", async (int id, MySQLContext db) =>
{
    return await db.Categories.FindAsync(id)
    is Category categoria ? Results.Ok(categoria) : Results.NotFound();
}).WithTags("Categorias");

app.MapGet("/categorias", async (MySQLContext db) =>
{
    return await db.Categories.ToListAsync();
}).WithTags("Categorias").RequireAuthorization();


app.MapPost("/categorias", async (Category category, MySQLContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{category.Id}", category);
}).WithTags("Categorias");

app.MapPut("/categorias/{id:int}", async (int id, Category categoria, MySQLContext db) =>
{
    var categoriaDB = await db.Categories.FindAsync(id);
    categoriaDB.Name = categoria.Name;
    categoriaDB.Description = categoria.Description;

    if (categoria.Id != id) return Results.BadRequest();
    if (categoriaDB is null) return Results.NotFound();
    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
}).WithTags("Categorias");

app.MapDelete("/categorias/{id:int}", async (int id, MySQLContext db) =>
{
    var categoria = await db.Categories.FindAsync(id);
    if (categoria is null) return Results.NotFound();


    db.Categories.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Categorias");
//-----------------CONTROLLERS PRODUTOS---------------------------

app.MapGet("/", () => "Catalogo de Produtos").ExcludeFromDescription().WithTags("Produtos");

app.MapGet("/produtos/{id:int}", async (int id, MySQLContext db) =>
{
    return await db.Products.FindAsync(id)
    is Product products ? Results.Ok(products) : Results.NotFound();
}).WithTags("Produtos");

app.MapGet("/produtos", async (MySQLContext db) =>
{
    return await db.Products.ToListAsync();
}).WithTags("Produtos").RequireAuthorization();

app.MapPost("/produtos", async (Product product, MySQLContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/produtos/{product.Id}", product);
}).WithTags("Produtos");

app.MapPut("/produtos/{id:int}", async (int id, Product product, MySQLContext db) =>
{
    var productDB = await db.Products.FindAsync(id);
    productDB.Name = productDB.Name;
    productDB.Description = productDB.Description;

    if (productDB.Id != id) return Results.BadRequest();
    if (productDB is null) return Results.NotFound();
    await db.SaveChangesAsync();
    return Results.Ok(productDB);
}).WithTags("Produtos");

app.MapDelete("/produtos/{id:int}", async (int id, MySQLContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();


    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Produtos");

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
