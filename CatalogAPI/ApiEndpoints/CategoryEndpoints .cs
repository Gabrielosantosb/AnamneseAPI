using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.ApiEndpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this WebApplication app)
        {            
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
        }
    }
}
