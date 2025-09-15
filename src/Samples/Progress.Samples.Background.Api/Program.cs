using Progress.Samples.Background.Api;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<BackgroundJob>();
builder.Services.AddHostedService<HostedService>();

var app = builder.Build();
app.MapGet("/info", (HttpContext ctx) =>
{
    ctx.Response.StatusCode = (int)HttpStatusCode.Accepted;
});

app.Run();