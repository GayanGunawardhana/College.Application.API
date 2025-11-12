using Azure.Core;
using College.App.Configurations;
using College.App.Data;
using College.App.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure the DbContext with SQL Server using the connection string from appsettings.json
builder.Services.AddDbContext<CollegeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeConnectionString")));


// Add services to the container.
//"options => options.ReturnHttpNotAcceptable=true" ensures that if the client requests a format(Ex:json or xml) that the server cannot produce, it will respond with a 406 Not Acceptable status code.
//Generally we use JSON formate for data reprsentation in web APIs.
//But if you want to support XML format also then you need to add the following line "builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable=true).AddXmlDataContractSerializerFormatters();"
//builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable=true).AddNewtonsoftJson();
builder.Services.AddControllers().AddNewtonsoftJson();

// These two are REQUIRED for Swagger
builder.Services.AddEndpointsApiExplorer();
//====================================================================================
// Swagger configuration for JWT Authentication
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        //Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
//====================================================================================

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi(); 
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
//====================================================================================================
//In middleware using a named policy or default policy we can enable CORS.
//builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
//    {
//        // AllowAnyOrigin: Allows requests from any origin
//        // AllowAnyMethod: Allows any HTTP method (GET, POST, PUT, DELETE, etc.)
//        // AllowAnyHeader: Allows any HTTP headers
//        // This is useful for development and testing, but be cautious when using it in production as it can expose your API to security risks.
//This is not safe for production environments.

//        policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    }));

//====================================================================================================
//Define one policy with specific origins(allowed origins) instead of allowing any origin.
//builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
//{
//    // Specify the allowed origins (This safe for production environment)
//    policy.WithOrigins("https://localhost:7243", "http://localhost:5243").AllowAnyHeader().AllowAnyMethod(); 

//}));

//====================================================================================================
//Multiple Named policies 
builder.Services.AddCors(options => 
{
    //Define a default policy
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    });
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    });
    options.AddPolicy("AllowOnlyLocalHost", policy =>
    {
        // Specify the allowed origins (This safe for production environment)
        policy.WithOrigins("https://localhost:7243", "http://localhost:5243").AllowAnyHeader().AllowAnyMethod();

    });
    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        // Specify the allowed origins (This safe for production environment)
        policy.WithOrigins("http://google.com", "http://gmail.com", "http://drive.google.com").AllowAnyHeader().AllowAnyMethod();

    });
});
//====================================================================================================
//JWT Authentication Setup can be added here(JWT Configuration)
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<String>("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{ 
    options.SaveToken = true; // Save the token in the AuthenticationProperties after a successful authorization
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // This ensures the token is issued by a trusted issuer
        ValidateAudience = true, // This ensures the token is intended for a specific audience
       // ValidateLifetime = true, // This ensures the token hasn't expired
        ValidateIssuerSigningKey = true, // This ensures the token's signature is valid
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Issuer from configuration
        ValidAudience = builder.Configuration["Jwt:Audience"], // Audience from configuration
        IssuerSigningKey = new SymmetricSecurityKey(key) // Secret key from configuration
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi(); // This generates OpenAPI docs (Swagger alternative)

    app.UseSwagger();     // generates the OpenAPI JSON
    app.UseSwaggerUI();   // enables Swagger UI page


}

app.UseHttpsRedirection();
//Enable Endpoint Routing Middleware
app.UseRouting();
//app.UseCors("AllowAll");
//default policy
app.UseCors();

app.UseAuthorization();
 
app.UseEndpoints(endpoints =>
{
    //This applies a CORS policy named "AllowOnlyLocalHost" only to this endpoint.
    //So only requests from localhost will be allowed if the CORS policy is configured that way.
    endpoints.MapGet("api/testingendpoint",
        context => context.Response.WriteAsync("Test Response"))
        .RequireCors("AllowOnlyLocalHost");

    //This maps all controller routes (e.g., [Route("api/[controller]")] endpoints).
    //These controller endpoints accept requests from any origin (depending on how "AllowAll" is defined in the CORS setup).
    endpoints.MapControllers()
             .RequireCors("AllowAll");

    //This one does not specify a CORS policy, so it will use the default CORS behavior (usually restricted or whatever is globally applied).
    //endpoints.MapGet("api/testendpoint2",
    //    context => context.Response.WriteAsync("Test Response 02"));

    endpoints.MapGet("api/testendpoint2",
       context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JwtSettings")));


});

//app.MapControllers(); 

app.Run();
