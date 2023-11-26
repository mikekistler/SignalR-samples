using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

app.MapHub<TicTacToeHub>("/hubs/tictactoe");

app.UseDefaultFiles();  // index.html
app.UseStaticFiles();

app.MapPost("/reset", async (IHubContext<TicTacToeHub> context) =>
{
    TicTacToeHub.PlayerCount = 0;
});

app.Run();
