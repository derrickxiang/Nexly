var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var configuration = builder.Configuration;
var env = builder.Environment;

// ---------- Serilog configuration (moved from Startup ctor) ----------
Log.Logger = new LoggerConfiguration()
    .Enrich.WithClientIp()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// ---------- Services (Startup.ConfigureServices) ----------


// Connection string
var dbStr = configuration.GetConnectionString("Default");

Console.WriteLine("Env: " + env.EnvironmentName);

// Controllers
builder.Services.AddControllers();

// Swagger
var environmentName = env.EnvironmentName;
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"Nexly.API.{environmentName}",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt auth header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.IncludeErrorDetails = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWTSettings:TokenKey"]))
        };
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy =>
    {
        policy.Requirements.Add(new IsAdminRequirement());
        // policy.RequireRole("Admin");
    });
});
builder.Services.AddTransient<IAuthorizationHandler, IsAdminRequirementHandler>();

// Application services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddSingleton<IApplicationState, ApplicationState>();

// DbContext
builder.Services.AddDbContext<NexlyDbContext>(opt =>
{
    opt.UseSqlServer(
        dbStr,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);
        });
});


// Repositories
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// MediatR
builder.Services.AddMediatR(typeof(Detail.Handler).Assembly);

// AutoMapper
builder.Services.AddAutoMapper(typeof(MyMappingProfile).Assembly);

// CORS
string[] allowedOrigins =
{
    "http://localhost:8000",    
};

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins(allowedOrigins);
    });
});

// Identity
builder.Services.AddIdentityCore<AppUser>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = false;
})
.AddRoles<AppRole>()
.AddEntityFrameworkStores<NexlyDbContext>()
.AddSignInManager<MySignInManager>()
.AddDefaultTokenProviders();

// Http client & context
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Misc services
builder.Services.AddSingleton<IServerIdProvider, ServerIdProvider>();

var myScheduler = new MyScheduler();
configuration.GetSection("MyScheduler").Bind(myScheduler);
builder.Services.AddSingleton(myScheduler);

// Hosted services
builder.Services.AddHostedService<DueDateCheckService>();
builder.Services.AddHostedService<RuntimeHostedService>();
builder.Services.AddHostedService<KeepAliveService>();


// ---------- Build app ----------
var app = builder.Build();

app.MapDefaultEndpoints();

// ---------- DB migration + seeding (from old Program.Main) ----------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<NexlyDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        await context.Database.MigrateAsync();
        await DbInitializer.Initialize(context, userManager, roleManager, logger);
    }
    catch (Exception ex)
    {   
        app.Logger.LogError(ex, "Problem migrating data");
    }
}

// ---------- Lifetime logging (from old Program.Main) ----------
app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Logger.LogInformation("The application has started at {time}", DateTime.Now);
});

app.Lifetime.ApplicationStopping.Register(() =>
{
    app.Logger.LogInformation("The application is stopping at {time}", DateTime.Now);
});

// ---------- Middleware pipeline (Startup.Configure) ----------

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Add(
            "cache-control", "public,max-age=31536000,immutable");
    }
});

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

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<MaintenanceModeMiddleware>();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

await app.RunAsync();

// ---------- Helpers ----------

static Task CustomHealthCheckResponseWriter(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";

    var response = new
    {
        status = report.Status.ToString(),
        results = report.Entries.Select(entry => new
        {
            key = entry.Key,
            status = entry.Value.Status.ToString(),
            description = entry.Value.Description
        })
    };

    return context.Response.WriteAsync(JsonSerializer.Serialize(response));
}

// Needed so ILogger<Program> works in top-level statements
public partial class Program { }
