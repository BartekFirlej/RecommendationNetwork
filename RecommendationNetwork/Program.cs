using Neo4j.Driver;
using RecommendationNetwork.Repositories;
using RecommendationNetwork.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDriver>(provider =>
{
    return GraphDatabase.Driver(
        new Uri("bolt://neo4jdb-server:7687/RecommendationNetwork"),
        AuthTokens.Basic("neo4j", "bartekfirlej1")
    );
});

builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IVoivodeshipService, VoivodeshipService>();
builder.Services.AddSingleton<IProductTypeService, ProductTypeService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IVoivodeshipRepository, VoivodeshipRepository>();
builder.Services.AddSingleton<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

builder.Services.AddSwaggerGen();

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
