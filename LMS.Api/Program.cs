using LMS.Api.Middleware;
using LMS.Api.Services;
using LMS.Application;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Services.AdminServices;
using LMS.Application.ViewModels;
using LMS.Infrastructure;
using LMS.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureSwaggerServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("LMS-CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseCors("LMS-CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
