using BlazorServerFrontend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton(new HttpClient());
builder.Services.AddSingleton<CustomersService>();
builder.Services.AddSingleton<ProductsService>();
builder.Services.AddSingleton<ProductTypesService>();
builder.Services.AddSingleton<RecommendationsService>();
builder.Services.AddSingleton<PurchasesService>();
builder.Services.AddSingleton<PurchaseProposalsService>();
builder.Services.AddSingleton<CartService>();
builder.Services.AddSingleton<VoivodeshipService>();
builder.Services.AddSingleton<AuthService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();