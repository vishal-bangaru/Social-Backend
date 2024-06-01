
using Microsoft.Data.SqlClient;
using PBLClass.Service;
using SQLRepository.Repository;
using System.Data;
using Microsoft.Azure.Cosmos;
using CloudinaryDotNet;
using Common.Services;
using Microsoft.AspNetCore.Hosting;
using Azure.Storage.Blobs;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<StudentInterface, StudentService>();
builder.Services.AddScoped<RepositoryInterface, CosmosRepository>();
builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage"));
});
builder.Services.AddScoped<FileServiceInterface, FileService>();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>() // This line loads secrets.json
    .AddEnvironmentVariables()
    .Build();


string connectionString = configuration.GetConnectionString("SqlConnection");

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connection = new SqlConnection(connectionString);
    return connection;
});


IConfiguration cosmosconfiguration = builder.Configuration.GetSection("CosmosDbConfiguration");
// Add services to the container.

builder.Services.AddSingleton<CosmosClient>((provider) =>
{
    var endpointuri = cosmosconfiguration["DatabaseUri"];
    var primarykey = cosmosconfiguration["Key"];
    var databaseId = cosmosconfiguration["DataBaseId"];
    var cosmosClient = new CosmosClient(endpointuri, primarykey);
    return cosmosClient;
});


builder.Services.AddSingleton<CloudinaryService>((provider) =>
{
    var cloudinaryConfig = new Account(
        configuration["Cloudinary:CloudName"],
        configuration["Cloudinary:ApiKey"],
        configuration["Cloudinary:ApiSecret"]
    );
    return new CloudinaryService(cloudinaryConfig.Cloud, cloudinaryConfig.ApiKey, cloudinaryConfig.ApiSecret);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

app.Run();