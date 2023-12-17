using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDriver>(provider =>
{
    return GraphDatabase.Driver(
        new Uri("bolt://localhost:7687/RecommendationNetwork"),
        AuthTokens.Basic("neo4j", "bartekfirlej1")
    );
});

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
