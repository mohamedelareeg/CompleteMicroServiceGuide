using CompleteMicroServiceGuide.Core.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CompleteMicroServiceGuide.Api.EndPoints
{
    public static class OrderEndpointsExtensions
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            // Endpoint to create an order
            app.MapPost("/order/create/{userId}", async (
                [FromServices] IOrderService orderService,
                [FromRoute] Guid userId) =>
            {
                try
                {
                    await orderService.CreateOrderAsync(userId);
                    return Results.Ok($"Order created for user ID: {userId}");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // Endpoint to cancel an order
            app.MapDelete("/order/cancel/{userId}/{orderId}", async (
                [FromServices] IOrderService orderService,
                [FromRoute] Guid userId,
                [FromRoute] Guid orderId) =>
            {
                try
                {
                    await orderService.CancelOrderAsync(orderId, userId);
                    return Results.Ok($"Order {orderId} canceled for user ID: {userId}");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // Endpoint to get orders for a given user ID
            app.MapGet("/order/{userId}", async (
                [FromServices] IOrderService orderService,
                [FromRoute] Guid userId) =>
            {
                try
                {
                    var orders = await orderService.GetOrdersAsync(userId);
                    return Results.Ok(orders);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // Endpoint to complete and submit an order
            app.MapPost("/order/submit/{userId}", async (
                [FromServices] IOrderService orderService,
                [FromRoute] Guid userId) =>
            {
                try
                {
                    await orderService.SubmitOrderAsync(userId);
                    return Results.Ok($"Order submitted for user ID: {userId}");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}
