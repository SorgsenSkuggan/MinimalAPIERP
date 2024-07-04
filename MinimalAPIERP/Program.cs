// FASE 1
using ERP.Api;
using ERP.Extensions;
using MinimalAPIERP.Infraestructure.Automapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery();
builder.Configuration.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

builder.Services
    .AddCustomSqlServerDb(builder.Configuration)
    .AddCustomHealthCheck(builder.Configuration)
    .AddCustomOpenApi(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// FASE 2
app.MapCustomHealthCheck(builder.Configuration);

app.UseAntiforgery();

app.DatabaseInit();

app.ConfigureSwagger();

app.MapStoreApi();
app.MapProductApi();
app.MapRaincheckApi();

app.Run();
