using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.ApiEndpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {

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
        }
    }
}
