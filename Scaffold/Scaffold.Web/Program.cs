var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseDefaultFiles();  // index.html
app.UseStaticFiles();

app.Run();
