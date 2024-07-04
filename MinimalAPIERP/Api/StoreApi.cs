using AutoMapper;
using ERP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MinimalAPIERP.Dtos;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using MinimalAPIERP.Configuration;

namespace ERP.Api;

internal static class StoreApi{
    public static RouteGroupBuilder MapStoreApi(this IEndpointRouteBuilder routes){
        var group = routes.MapGroup("/erp")
            .WithTags("Store Api");

        group.MapGet("/user", (ClaimsPrincipal user) =>
        {
            return user.Identity;

        })
        .WithOpenApi();

        group.MapGet("/stores/{storeguid}", async Task<Results<Ok<StoreDto>, NotFound>> (Guid storeguid, AppDbContext db, IMapper mapper) => {
            return mapper.Map<StoreDto>(await db.Stores.FirstOrDefaultAsync(m => m.StoreGuid == storeguid)) 
                is StoreDto store
                    ? TypedResults.Ok(store)
                    : TypedResults.NotFound();
        })
        .WithName("GetStore")
        .WithOpenApi();

        //group.MapGet("/storea", async Task<Results<Ok<IList<Store>>, NotFound>> (AppDbContext db) =>
        //    await db.Stores.ToListAsync()
        //        is IList<Store> stores
        //            ? TypedResults.Ok(stores)
        //            : TypedResults.NotFound())
        //    .WithOpenApi();


        //group.MapGet("/storeb", async Task<Results<Ok<IList<Store>>, NotFound>> (AppDbContext db, int pageSize = 10, int page = 0) =>
        //    await db.Stores.Skip(page * pageSize).Take(pageSize).ToListAsync()
        //        is IList<Store> stores
        //            ? TypedResults.Ok(stores)
        //            : TypedResults.NotFound())
        //    .WithOpenApi();

        group.MapGet("/storesc1", async Task<Results<Ok<IList<StoreDto>>, NotFound>> (AppDbContext db, IMapper mapper, int pageSize = 10, int page = 0) =>
            mapper.Map<StoreDto>(await db.Stores
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(store => new { store.StoreGuid, store.Name })
            .ToListAsync())
                is IList<StoreDto> stores
                    ? TypedResults.Ok(stores)
                    : TypedResults.NotFound())
            .WithName("GetStoreListPaginated")
            .WithOpenApi();

        //group.MapGet("/storec2", async  (AppDbContext db, int pageSize = 10, int page = 0) =>
        //{
        //    var data = await db.Stores
        //        .Skip(page * pageSize)
        //        .Take(pageSize)
        //        .Include(s => s.Rainchecks)
        //        .Select(store => new { store.StoreId, store.Name })
        //        .ToListAsync();

        //    return data.Any()
        //        ? Results.Ok(data)
        //        : Results.NotFound();
        //})
        //.WithOpenApi();

        group.MapGet("/storesd", async Task<Results<Ok<IList<StoreDto>>, NotFound>> (AppDbContext db, IMapper mapper) =>
            mapper.Map<StoreDto>(await db.Stores.Include(s => s.Rainchecks).ToListAsync())
                is IList<StoreDto> stores
                    ? TypedResults.Ok(stores)
                    : TypedResults.NotFound())
            .WithName("GetStoreListRainchecks")
            .WithOpenApi();

        //group.MapGet("/storee", async (AppDbContext db) =>
        //    await db.Stores.Include(s => s.Rainchecks).ToListAsync()
        //        is IList<Store> stores
        //            ? Results.Json(stores, JsonConfiguration.GetAPIJsonSerializsrOptions())
        //            : Results.NotFound())
        //    .WithOpenApi();

        group.MapGet("/storesf", async (AppDbContext db, IMapper mapper) =>
            mapper.Map<StoreDto>(await db.Stores.Include(s => s.Rainchecks).ToListAsync())
                is IList<StoreDto> stores
                    ? Results.Json(stores, JsonConfiguration.GetAPIJsonSerializerOptions())
                    : Results.NotFound())
            .WithName("GetStoreListWithJsonOptions")
            .WithOpenApi();

        group.MapPost("/stores", async Task<Created<Store>> (AppDbContext db, Store newStore) => {
            var store = new Store{
                StoreId = db.Stores.Last().StoreId + 1,
                StoreGuid = Guid.NewGuid(),
                Name = newStore.Name
            };

            db.Stores.Add(store);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/stores/{store.StoreGuid}", store);
        })
        .WithName("NewStore")
        .WithOpenApi();

        group.MapDelete("/stores/{guid}", async Task<Results<NotFound, Ok>> (AppDbContext db, Guid guid) =>
        {
            var rowsAffected = await db.Stores.Where(s => s.StoreGuid == guid)
                                             .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        })
        .WithName("DeleteStore")
        .WithOpenApi();

        group.MapPut("/stores/{guid}", async Task<Results<Ok, NotFound, BadRequest>> (AppDbContext db, Guid guid, Store store) => {
            if (guid != store.StoreGuid){
                return TypedResults.BadRequest();
            }

            var rowsAffected = await db.Stores.Where(s => s.StoreGuid == guid)
                                             .ExecuteUpdateAsync(updates => updates
                                                       .SetProperty(s => s.Name, store.Name));

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();

        })
        .WithName("UpdateStore")
        .WithOpenApi();

        return group;
    }
}
