using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PaymentTransaction.Data;
using PaymentTransaction.Mappings;
using PaymentTransaction.Repositories;
using System.Reflection;
using PaymentTransaction.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PaymentTransaction.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// // Register the Swagger generator with annotations support
// builder.Services.AddSwaggerGen(options =>
// {
//     options.EnableAnnotations(); // Enable processing of Swagger annotations

//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Version = "v1",
//         Title = "Your API Title",
//         Description = "Your API Description"
//     });

//     // Include XML comments if you have them
//     var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
//     if (File.Exists(xmlPath))
//     {
//         options.IncludeXmlComments(xmlPath);
//     }

//     // Register the custom schema filter
//     options.SchemaFilter<SwaggerSchemaExampleFilter>();

//     // ✅ Register the operation filter that adds Idempotency-Key to Swagger UI
//     options.OperationFilter<AddIdempotencyKeyHeaderParameter>();
// });

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Your API Title",
        Description = "Your API Description"
    });

    // Include XML comments if you have them
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Register your existing filters
    options.SchemaFilter<SwaggerSchemaExampleFilter>();
    options.OperationFilter<AddIdempotencyKeyHeaderParameter>();

    // ✅ Add security definitions for JWT and API key

    // // JWT Bearer
    // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    // {
    //     Name = "Authorization",
    //     Type = SecuritySchemeType.ApiKey,
    //     Scheme = "Bearer",
    //     BearerFormat = "JWT",
    //     In = ParameterLocation.Header,
    //     Description = "Enter JWT token with **Bearer** prefix. Example: `Bearer {token}`"
    // });

    // API Key
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "x-api-key",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter your API key in the `x-api-key` header."
    });

    // Apply both globally (so they’re available to all endpoints)
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            Array.Empty<string>()
        }
    });
});


// Register IdempotencyFilter
builder.Services.AddScoped<IdempotencyFilter>();

// Db Class DI
builder.Services.AddDbContext<PaymentTransactionDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentTransactionConnectionString")));

// Auth Db Class DI
builder.Services.AddDbContext<PaymentTransactionAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentAuthConnectionString")));


// Repository services
builder.Services.AddScoped<IProviderRepository, SQLProviderRepository>();
builder.Services.AddScoped<ICurrencyRepository, SQLCurrencyRepository>();
builder.Services.AddScoped<IStatusRepository, SQLStatusRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, SQLPaymentMethodRepository>();
builder.Services.AddScoped<ITransactionRepository, SQLTransactionRepository>();
builder.Services.AddScoped<IStatusRepository, SQLStatusRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var jwtKey = builder.Configuration["Jwt:Key"] 
             ?? throw new InvalidOperationException("JWT Key is not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        });

var app = builder.Build();

// Dev Ex Page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ← put it right here
}
else
{
    app.UseExceptionHandler("/error");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// JWT
app.UseAuthentication(); 
// API key or JWT
app.UseMiddleware<CombinedAuthMiddleware>(); 
app.UseAuthorization();

app.MapControllers();

app.Run();