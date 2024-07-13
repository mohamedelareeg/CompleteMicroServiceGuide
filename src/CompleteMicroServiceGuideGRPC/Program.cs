using CompleteMicroServiceGuide.Core.Services.Abstractions;
using CompleteMicroServiceGuide.Core.Services;
using CompleteMicroServiceGuideGRPC.Services;
using Marten;
using Newtonsoft.Json;
using Weasel.Core;
using CompleteMicroServiceGuide.Core.Projectors;
using Marten.Events.Projections;
using RabbitMQ.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
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
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IInventoryService, CompleteMicroServiceGuide.Core.Services.InventoryService>();
builder.Services.AddScoped<IProductTransactionService, ProductTransactionService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CompleteMicroServiceGuideGRPC.Services.InventoryService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
