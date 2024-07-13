using CompleteMicroServiceGuide.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CompleteMicroServiceGuide.Api.EndPoints
{
    public static class CartEndpointsExtensions
    {
        public static void MapCartEndpoints(this WebApplication app)
        {
            // Endpoint to add items to the shopping cart by product ID, and price
            app.MapPost("/cart/add/{userId}/{productId}/{price}", async (
                [FromServices] ICartService cartService,
                [FromRoute] Guid userId,
                [FromRoute] Guid productId,
                [FromRoute] decimal price) =>
            {
                return await cartService.AddItemToCartAsync(userId, productId, price);
            });


            // Endpoint to remove items from the shopping cart by product ID
            app.MapDelete("/cart/remove/{userId}/{productId}", async (
                [FromServices] ICartService cartService,
                [FromRoute] Guid userId,
                [FromRoute] Guid productId) =>
            {
                return await cartService.RemoveItemFromCartAsync(userId, productId);
            });

            // Endpoint to update the quantity of an item in the cart by product ID and new quantity
            app.MapPut("/cart/update-quantity/{userId}/{productId}/{quantity}", async (
                [FromServices] ICartService cartService,
                [FromRoute] Guid userId,
                [FromRoute] Guid productId,
                [FromRoute] int quantity) =>
            {
                return await cartService.UpdateCartItemQuantityAsync(userId, productId, quantity);
            });

            // Endpoint to update the price of an item in the cart by product ID and new price
            app.MapPut("/cart/update-price/{userId}/{productId}/{price}", async (
                [FromServices] ICartService cartService,
                [FromRoute] Guid userId,
                [FromRoute] Guid productId,
                [FromRoute] decimal price) =>
            {
                return await cartService.UpdateCartItemPriceAsync(userId, productId, price);
            });

            // Endpoint to update shipping information with specific address and phone number inputs
            app.MapPut("/cart/update-shipping/{userId}/{address}/{phoneNumber}", async (
                [FromServices] ICartService cartService,
                [FromRoute] Guid userId,
                [FromRoute] string address,
                [FromRoute] string phoneNumber) =>
            {
                return await cartService.UpdateShippingInformationAsync(userId, address, phoneNumber);
            });

            // Endpoint to get all cart items and their total for a given user ID
            app.MapGet("/cart/{userId}", async (
                HttpContext context,
                [FromServices] ICartService cartService,
                [FromRoute] Guid userId) =>
            {
                var result = await cartService.GetCartDetailsAsync(userId);
                await context.Response.WriteAsJsonAsync(result);
            });

            // Endpoint to get the event stream for a user's cart
            app.MapGet("/cart/events/{userId}", async (
                [FromServices] ICartService cartService,
                [FromRoute] Guid userId) =>
            {
                return await cartService.GetCartEventStreamAsync(userId);
            });
        }
    }
}
