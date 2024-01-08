using Neo4j.Driver;
using RecommendationNetwork.Repositories;
using RecommendationNetwork.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDriver>(provider =>
{
    return GraphDatabase.Driver(
        new Uri("bolt://127.0.0.1:7687"),
        AuthTokens.Basic("neo4j", "bartekfirlej1")
    );
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
