using ProductStore;
using ProductStore.Repositories;
using ProductStore.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IPurchaseDetailRepository, PurchaseDetailRepository>();
builder.Services.AddScoped<IPurchaseProposalRepository, PurchaseProposalRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IVoivodeshipRepository, VoivodeshipRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IPurchaseDetailService, PurchaseDetailService>();
builder.Services.AddScoped<IPurchaseProposalService, PurchaseProposalService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IVoivodeshipService, VoivodeshipService>();

builder.Services.AddAutoMapper(typeof(StoreMapper));

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

builder.Services.AddSingleton<RabbitMqPublisher>();

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
