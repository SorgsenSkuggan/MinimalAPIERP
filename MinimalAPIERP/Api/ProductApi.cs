using AutoMapper;
using ERP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MinimalAPIERP.Configuration;
using MinimalAPIERP.Dtos;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Api;

internal static class ProductApi
{
    public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/erp")
            .WithTags("Product Api");

        group.MapGet("/products/{productguid}", async Task<Results<Ok<ProductDto>, NotFound>> (Guid productguid, AppDbContext db, IMapper mapper) => {
            return mapper.Map<ProductDto>(await db.Products.FirstOrDefaultAsync(p => p.ProductGuid == productguid))
                is ProductDto product
                    ? TypedResults.Ok(product)
                    : TypedResults.NotFound();
        })
        .WithName("GetPoduct")
        .WithOpenApi();

        group.MapGet("/products/", async (AppDbContext db, IMapper mapper) =>
            mapper.Map<IList<ProductDto>>( await db.Products.ToListAsync()
                is IList<ProductDto> products
                    ? Results.Json(products, JsonConfiguration.GetAPIJsonSerializerOptions())
                    : Results.NotFound())
            ).WithOpenApi();

        group.MapPost("/products/", async Task<Created<Product>> (AppDbContext db, Product newProduct, IMapper mapper) => {
            var product = new Product{
                ProductId = db.Products.Last().ProductId + 1,
                ProductGuid = Guid.NewGuid(),
                SkuNumber = newProduct.SkuNumber,
                CategoryGuid = newProduct.CategoryGuid,
                RecommendationId = newProduct.RecommendationId,
                Title = newProduct.Title,
                Price = newProduct.Price,
                SalePrice = newProduct.SalePrice,
                ProductArtUrl = newProduct.ProductArtUrl,
                Description = newProduct.Description,
                Created = newProduct.Created.ToString().IsNullOrEmpty() ? DateTime.Now : newProduct.Created,
                ProductDetails = newProduct.ProductDetails,
                Inventory = newProduct.Inventory,
                LeadTime = newProduct.LeadTime
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/products/{product.ProductGuid}", product);
        })
        .WithName("NewProduct")
        .WithOpenApi();

        group.MapDelete("products/{guid}", async Task<Results<NotFound, Ok>> (AppDbContext db, Guid guid) =>
        {
            var rowsAffected = await db.Products.Where(p => p.ProductGuid == guid)
                                             .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        })
        .WithName("DeleteProduct")
        .WithOpenApi();

        group.MapPut("products/{guid}", async Task<Results<Ok, NotFound, BadRequest>> (AppDbContext db, Guid guid, Product product) =>
        {
            if (guid != product.ProductGuid){
                return TypedResults.BadRequest();
            }

            var rowsAffected = await db.Products.Where(p => p.ProductGuid == guid)
                                             .ExecuteUpdateAsync(updates => updates
                                                 .SetProperty(p => p.SkuNumber, product.SkuNumber)
                                                 .SetProperty(p => p.CategoryGuid, product.CategoryGuid)
                                                 .SetProperty(p => p.RecommendationId, product.RecommendationId)
                                                 .SetProperty(p => p.Title, product.Title)
                                                 .SetProperty(p => p.Price, product.Price)
                                                 .SetProperty(p => p.SalePrice, product.SalePrice)
                                                 .SetProperty(p => p.ProductArtUrl, product.ProductArtUrl)
                                                 .SetProperty(p => p.Description, product.Description)
                                                 .SetProperty(p => p.ProductDetails, product.ProductDetails)
                                                 .SetProperty(p => p.Inventory, product.Inventory)
                                                 .SetProperty(p => p.LeadTime, product.LeadTime)
            );

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();

        })
        .WithName("UpdateProduct")
        .WithOpenApi();

        return group;
    }
}
