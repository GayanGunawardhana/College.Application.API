using College.App.Configurations;
using College.App.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CollegeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeConnectionString")));
// Configure the DbContext with SQL Server using the connection string from appsettings.json


// Add services to the container.
//"options => options.ReturnHttpNotAcceptable=true" ensures that if the client requests a format(Ex:json or xml) that the server cannot produce, it will respond with a 406 Not Acceptable status code.
//Generally we use JSON formate for data reprsentation in web APIs.
//But if you want to support XML format also then you need to add the following line "builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable=true).AddXmlDataContractSerializerFormatters();"
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable=true).AddNewtonsoftJson();

// These two are REQUIRED for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi(); 
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi(); // This generates OpenAPI docs (Swagger alternative)

    app.UseSwagger();     // generates the OpenAPI JSON
    app.UseSwaggerUI();   // enables Swagger UI page


}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
