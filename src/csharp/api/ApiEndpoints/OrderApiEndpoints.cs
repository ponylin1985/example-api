using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Extensions;
using Example.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using RestApiResult = Microsoft.AspNetCore.Http.HttpResults.Results<
    Microsoft.AspNetCore.Http.HttpResults.Ok<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.BadRequest<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.InternalServerError<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.StatusCodeHttpResult>;

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
        MapUpdateOrderInstructions(group);
        return app;
    }

    /// <summary>
    /// Maps the GetOrder endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapGetOrder(RouteGroupBuilder group)
    {
        group.MapGet("/{id:long:min(1)}", async Task<RestApiResult> (long id, IPatientOrderService orderService) =>
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
        group.MapPost("/", async Task<RestApiResult> (
            CreatePatientOrderRequest request,
            IPatientOrderService orderService,
            IOutputCacheStore cacheStore,
            IValidator<CreatePatientOrderRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResult<IDictionary<string, string[]>>
                {
                    Success = false,
                    Code = ApiCode.InvalidRequest,
                    Data = validationResult.ToDictionary(),
                    Message = "Invalid request data.",
                }.ToHttpResult();
            }

            var result = await orderService
                .AddPatientOrderAsync(request)
                .TapOnSuccessAsync(async () => await EvictOrderRelatedCaches(cacheStore));
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("AddPatientOrder")
        .WithDescription("Create a new order for a patient.");
    }

    /// <summary>
    /// Maps the UpdateOrderInstructions endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapUpdateOrderInstructions(RouteGroupBuilder group)
    {
        group.MapPut("/{id:long:min(1)}", async Task<RestApiResult> (
            long id,
            [FromBody] UpdateOrderInstructionseRequest request,
            IPatientOrderService orderService,
            IOutputCacheStore cacheStore,
            IValidator<UpdateOrderInstructionseRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResult<IDictionary<string, string[]>>
                {
                    Success = false,
                    Code = ApiCode.InvalidRequest,
                    Data = validationResult.ToDictionary(),
                    Message = "Invalid request data.",
                }.ToHttpResult();
            }

            var result = await orderService
                .UpdateInstructionsAsync(id, request.Instructions!, request.UserId!)
                .TapOnSuccessAsync(async () => await EvictOrderRelatedCaches(cacheStore));
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("UpdatePatientOrderInstructions")
        .WithDescription("Update the instructions of an existing patient order.");
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
