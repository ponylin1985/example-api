using Example.Api.Dtos.Requests;
using Example.Api.Extensions;
using Example.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Endpoints;

public static class OrderApiEndpoints
{
    public static IEndpointRouteBuilder MapOrderApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/orders")
            .WithTags("Orders");

        group.MapGet("/{id}", async (
            long id,
            IOrderService orderService,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderApiEndpoints");
            logger.LogInformation("Received request to get order with ID: {OrderId}", id);

            var result = await orderService.GetOrderAsync(id);

            if (!result.Success)
            {
                logger.LogWarning("Failed to retrieve order: {Message}", result.Message);
            }
            else
            {
                logger.LogInformation("Successfully retrieved order with ID: {OrderId}", result.Data?.Id);
            }

            return result.ToHttpResult();
        })
        .WithName("GetOrderById")
        .WithDescription("Get an order by its identifier.");

        group.MapPost("/", async (
            CreateOrderRequest request,
            IOrderService orderService,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderApiEndpoints");
            logger.LogInformation("Received request to create a new order for Patient ID: {PatientId}", request.PatientId);

            var result = await orderService.CreateOrderAsync(request);

            if (!result.Success)
            {
                logger.LogWarning("Failed to create order: {Message}", result.Message);
            }
            else
            {
                logger.LogInformation("Successfully created order with ID: {OrderId}", result.Data?.Id);
            }

            return result.ToHttpResult();
        })
        .WithName("CreateOrder")
        .WithDescription("Create a new order.");

        group.MapPut("/{id}", async (
            long id,
            [FromBody] UpdateOrderMessageRequest request,
            IOrderService orderService,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderApiEndpoints");
            logger.LogInformation("Received request to update message for order with ID: {OrderId}", id);

            var result = await orderService.UpdateOrderMessageAsync(id, request.Message);

            if (!result.Success)
            {
                logger.LogWarning("Failed to update order message: {Message}", result.Message);
            }
            else
            {
                logger.LogInformation("Successfully updated message for order with ID: {OrderId}", result.Data?.Id);
            }

            return result.ToHttpResult();
        })
        .WithName("UpdateOrderMessage")
        .WithDescription("Update the message of an existing order.");

        return app;
    }
}
