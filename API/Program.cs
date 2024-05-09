using Antopia.API.Application;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


var proveedor = builder.Services.BuildServiceProvider();
var configuration = proveedor.GetRequiredService<IConfiguration>();

builder.Services.AddCors(opciones =>
{
    var fronedUrl = configuration.GetValue<string>("frontendUrl");
    opciones.AddDefaultPolicy(builder => {
        builder.WithOrigins(fronedUrl).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

builder.Services.AddStartupSetup(builder.Configuration);

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.UseCors();
app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();
