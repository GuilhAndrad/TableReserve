using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TableReserve.API.Filters;
using TableReserve.API.Token;
using TableReserve.Application;
using TableReserve.Communication.Responses;
using TableReserve.Domain.Repositories.User;
using TableReserve.Domain.Security.Tokens;
using TableReserve.Exception;
using TableReserve.Infrastructure;
using TableReserve.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter only your access token. Swagger will add 'Bearer' automatically.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(openApiDocument =>
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", openApiDocument), []
            }
        };
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IAccessTokenProvider, HttpContextTokenProvider>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

builder.Services.AddRouting(options => options.LowercaseUrls = true);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtoptions =>
    {
        var signingKey = builder.Configuration.GetValue<string>("Jwt:SigningKey")!;

        jwtoptions.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        jwtoptions.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var subject = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(subject, out var userId) == false)
                {
                    context.Fail("Invalid token subject.");

                    return;
                }

                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReader>();

                var userExists = await userRepository.ExistActiveUserWithId(userId, context.HttpContext.RequestAborted);
                if (userExists == false)
                {
                    context.Fail("User not found or inactive");
                }
            },
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = context.AuthenticateFailure switch
                {
                    null => new ErrorResponse(MessagesExceptionResource.ACCESS_TOKEN_REQUIRED_VALIDATION),
                    SecurityTokenExpiredException => new ErrorResponse("Token expired", accestokenExpired: true),
                    _ => new ErrorResponse(MessagesExceptionResource.ACCESS_DENIED_VALIDATION),
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await ExecuteMigrations();

app.Run();

async Task ExecuteMigrations()
{
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
    DatabaseMigration.RunnerMigrations(scope.ServiceProvider);
}