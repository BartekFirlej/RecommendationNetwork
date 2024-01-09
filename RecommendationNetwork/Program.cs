using Microsoft.AspNetCore.Connections;
using Neo4j.Driver;
using RecommendationNetwork.Repositories;
using RecommendationNetwork.Services;
using RabbitMQ.Client;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDriver>(provider =>
{
    return GraphDatabase.Driver(
        new Uri("bolt://neo4jdb-server:7687/RecommendationNetwork"),
        AuthTokens.Basic("neo4j", "bartekfirlej1")
    );
});

builder.Services.AddSingleton<IConnection>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    var factory = new ConnectionFactory
    {
        HostName = "rabbitmq-container",
        Port = 5672,
        UserName = "admin",
        Password = "ADMIN"
    };

    return factory.CreateConnection();
});

builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IVoivodeshipService, VoivodeshipService>();
builder.Services.AddSingleton<IProductTypeService, ProductTypeService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IPurchaseService, PurchaseService>();
builder.Services.AddSingleton<IPurchaseDetailService, PurchaseDetailService>();
builder.Services.AddSingleton<IPurchaseProposalService, PurchaseProposalService>();
builder.Services.AddSingleton<ICustomerRecommendationService, CustomerRecommendationService>();
builder.Services.AddSingleton<IPurchaseRecommendationService, PurchaseRecommendationService>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IVoivodeshipRepository, VoivodeshipRepository>();
builder.Services.AddSingleton<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddSingleton<IPurchaseDetailRepository, PurchaseDetailRepository>();
builder.Services.AddSingleton<IPurchaseProposalRepository, PurchaseProposalRepository>();
builder.Services.AddSingleton<ICustomerRecommendationRepository, CustomerRecommendationRepository>();
builder.Services.AddSingleton<IPurchaseRecommendationRepository, PurchaseRecommendationRepository>();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RabbitMqConsumer>();

builder.Services.AddHostedService<RabbitMqBackgroundService>();

builder.Services.AddSingleton<RabbitMqPublisher>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World");

app.Run();
