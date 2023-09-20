using Api.BusinessLogic;
using Api.Cache;
using Api.Services;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<InitializeCacheService>();
builder.Services.AddSingleton<IParser, ParserLogic>();
builder.Services.AddSingleton<ISearcher, BinarySearchLogic>();

builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
    var fileNameEntities = $"{typeof(GeoIPCache).Assembly.GetName().Name}.xml";
    var filePathEntities = Path.Combine(AppContext.BaseDirectory, fileNameEntities);
    options.IncludeXmlComments(filePathEntities);
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "GeoIP API",
        Description = "An ASP.NET Core Web API for searching Geo-IP information",
        //License = new OpenApiLicense
        //{
        //    Name = "Specification",
            //Url = new Uri("https://www.metaquotes.net/ru/company/vacancies/tests/dot-net")
        //}
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                            .WithOrigins("http://pavel.solutions/spa", "http://pavel.solutions", "http://localhost/spa")
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();