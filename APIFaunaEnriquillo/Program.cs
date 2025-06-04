using APIFaunaEnriquillo.Core.DomainLayer;
using APIFaunaEnriquillo.Core.AplicationLayer;
using APIFaunaEnriquillo.InfraestructureLayer.Identity;
using APIFaunaEnriquillo.InfrastructureLayer.Shared;
using APIFaunaEnriquillo.Extensions;
using APIFaunaEnriquillo.InfraestructureLayer.Identity.Models;
using APIFaunaEnriquillo.InfraestructureLayer.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager? config = builder.Configuration;
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
                 .ReadFrom.Services(services);
});

// Add services to the container.
//Lectura connection string
var connectionString = builder.Configuration.GetConnectionString("FaunaEnriquillo");

//Inyeccion de dependencias
builder.Services.AddPersistenceMethod(config);
builder.Services.AddAplicationLayer();
builder.Services.AddIdentityLayer(config);
builder.Services.AddSharedLayer(config);

//Extensions
builder.Services.AddSwaggerGen();
builder.Services.AddVersionIng();
builder.Services.AddExceptions();
builder.Services.AddValidations();
builder.Services.AddLimit();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow", policy =>
    {
        policy.WithOrigins("http://localhost:5081")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("Allow");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultOwnerRoles.SeedAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during role seeding.");
    }
}

app.UseExceptionHandler(_ => { });
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
