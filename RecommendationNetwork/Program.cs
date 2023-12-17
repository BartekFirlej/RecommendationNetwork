var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<Neo4jService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var neo4jUri = configuration.GetConnectionString("Neo4jConnection");
    var neo4jUser = configuration.GetConnectionString("Neo4jUser");
    var neo4jPassword = configuration.GetConnectionString("Neo4jPassword");

    return new Neo4jService(neo4jUri, neo4jUser, neo4jPassword);
});

builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World");

app.Run();
