using GameStore.Api.Data;
using GameStore.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();


builder.AddGameStoreDB();
var app = builder.Build();


app.MapGamesEndpoints();
app.MigrateDB();


app.Run();
