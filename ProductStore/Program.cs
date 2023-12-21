using ProductStore.Repositories;
using ProductStore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddSingleton<IPurchaseDetailRepository, PurchaseDetailRepository>();
builder.Services.AddSingleton<IPurchaseProposalRepository, PurchaseProposalRepository>();
builder.Services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddSingleton<IVoivodeshipRepository, VoivodeshipRepository>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IProductTypeService, ProductTypeService>();
builder.Services.AddSingleton<IPurchaseDetailService, PurchaseDetailService>();
builder.Services.AddSingleton<IPurchaseProposalService, PurchaseProposalService>();
builder.Services.AddSingleton<IPurchaseService, PurchaseService>();
builder.Services.AddSingleton<IVoivodeshipService, VoivodeshipService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

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


app.MapGet("/", () => "Hello World!");

app.Run();
