using CompleteMicroServiceGuide.Api.EndPoints;
using CompleteMicroServiceGuide.Core.Projectors;
using CompleteMicroServiceGuide.Core.Services;
using CompleteMicroServiceGuide.Core.Services.Abstractions;
using Marten;
using Marten.Events.Projections;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Weasel.Core;
using RabbitMQ.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

var serializer = new Marten.Services.JsonNetSerializer();
serializer.EnumStorage = EnumStorage.AsString;
serializer.Customize(_ =>
{
    _.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
    _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
});

builder.Services.AddMarten(o =>
{
    o.Connection(builder.Configuration.GetConnectionString("Default"));
    o.Serializer(serializer);
    //o.Projections.Add<WarehouseProjection>(ProjectionLifecycle.Inline);
    o.Projections.Add<InventoryProjector>(ProjectionLifecycle.Inline);
    o.Projections.Add<ProductTransactionProjection>(ProjectionLifecycle.Inline);
    o.Projections.Add<CartProjector>(ProjectionLifecycle.Inline);
    o.Projections.Add<OrderSummaryProjector>(ProjectionLifecycle.Inline);
});

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IProductTransactionService, ProductTransactionService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory API", Version = "v1" });

    c.TagActionsBy(api => new List<string> { api.RelativePath.Split('/')[0] });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API V1");
    });
}
app.MapCartEndpoints();
app.MapOrderEndpoints();
app.MapInventoryEndpoints();

app.Run();

