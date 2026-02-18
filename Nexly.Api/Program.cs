using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Nexly.Api.MiddleWares;
using Nexly.Application.Articles.Queries;
using Nexly.Application.Articles.Validators;
using Nexly.Application.Core;
using Nexly.Domain;
using Nexly.Infrastructure.data;
using Serilog;
using System;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ---------- Serilog configuration (moved from Startup ctor) ----------
Log.Logger = new LoggerConfiguration()
    .Enrich.WithClientIp()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

//// Add services to the container.
//builder.Services.AddControllers(opt =>
//{
//    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//    opt.Filters.Add(new AuthorizeFilter(policy));
//});
builder.Services.AddControllers();
builder.Services.AddDbContext<NexlyDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

Console.WriteLine("Env: " + builder.Environment.EnvironmentName);

builder.Services.AddCors();
//builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetArticleList>();
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.LicenseKey = builder.Configuration["Licences:MediatR"];
});
//builder.Services.AddHttpClient<ResendClient>();
//builder.Services.Configure<ResendClientOptions>(opt =>
//{
//    opt.ApiToken = builder.Configuration["Resend:ApiToken"]!;
//});
//builder.Services.AddTransient<IResend, ResendClient>();
//builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
//builder.Services.AddScoped<IUserAccessor, UserAccessor>();
//builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = builder.Configuration["Licences:MediatR"];
}, typeof(MappingProfiles));
builder.Services.AddValidatorsFromAssemblyContaining<CreateArticleValidator>();
builder.Services.AddIdentityApiEndpoints<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NexlyDbContext>();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"Nexly.API.{builder.Environment.EnvironmentName}",
        Version = "v1"
    });

    //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    Description = "Jwt auth header",
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer"
    //});

});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

// if (env.IsDevelopment())
// {
app.UseDeveloperExceptionPage();
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "Nexly.API v1");
    c.RoutePrefix = "swagger";
});
// }

app.UseRouting();
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:5000"));

//app.UseAuthentication();
//app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
//app.MapGroup("api").MapIdentityApi<User>();
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<NexlyDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedData(context, userManager);
}
catch (Exception ex)
{
   app.Logger.LogError(ex, "An error occurred during migration");
}

app.Run();
