using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Extensions;
using Example.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Example.Api.Endpoints;

/// <summary>
/// API endpoints extensions for patient's orders.
/// </summary>
public static class OrderApiEndpoints
{
    /// <summary>
    /// Maps order-related API endpoints.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapOrderApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/orders")
            .WithTags("Orders");
        MapGetOrder(group);
        MapCreateOrder(group);
        MapUpdateOrderMessage(group);
        return app;
    }

    /// <summary>
    /// Maps the GetOrder endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapGetOrder(RouteGroupBuilder group)
    {
        group.MapGet("/{id:long:min(1)}", async (
            long id,
            IOrderService orderService) =>
        {
            var result = await orderService.GetOrderAsync(id);
            return result.ToHttpResult();
        })
        .WithName("GetOrderById")
        .WithDescription("Get an order by its identifier.")
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .CacheOutput(policy => policy
            .Expire(TimeSpan.FromMinutes(5))
            .SetVaryByRouteValue("id")
            .Tag("order-detail"));
    }

    /// <summary>
    /// Maps the CreateOrder endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapCreateOrder(RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            CreateOrderRequest request,
            IOrderService orderService,
            IOutputCacheStore cacheStore) =>
        {
            var result = await orderService
                .CreateOrderAsync(request)
                .TapOnSuccessAsync(async () => await EvictOrderRelatedCaches(cacheStore));
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("CreateOrder")
        .WithDescription("Create a new order.");
    }

    /// <summary>
    /// Maps the UpdateOrderMessage endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapUpdateOrderMessage(RouteGroupBuilder group)
    {
        group.MapPut("/{id:long:min(1)}", async (
            long id,
            [FromBody] UpdateOrderMessageRequest request,
            IOrderService orderService,
            IOutputCacheStore cacheStore) =>
        {
            var result = await orderService
                .UpdateMessageAsync(id, request.Message)
                .TapOnSuccessAsync(async () => await EvictOrderRelatedCaches(cacheStore));
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("UpdateOrderMessage")
        .WithDescription("Update the message of an existing order.");
    }

    /// <summary>
    /// Evicts caches related to orders and patients.
    /// </summary>
    /// <param name="cacheStore"></param>
    /// <returns></returns>
    private static async Task EvictOrderRelatedCaches(IOutputCacheStore cacheStore)
    {
        await cacheStore.EvictByTagAsync("patients", default);
        await cacheStore.EvictByTagAsync("patient-detail", default);
        await cacheStore.EvictByTagAsync("order-detail", default);
    }
}
