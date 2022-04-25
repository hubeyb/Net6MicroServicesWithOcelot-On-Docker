using KloiaCase.DataAccess;
using KloiaCase.DataAccess.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<INetCoreMicroServicesDBContext, NetCoreMicroServicesDBContext>(options => {
    var connectionStr = builder.Configuration.GetConnectionString("NetCoreMicroServicesDBConnection");
    options.UseSqlServer(connectionStr);
}, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

builder.Services.AddControllers().AddOData(options => options.Select().Filter().OrderBy());

builder.Services.AddScoped<IArticleService, ArticlesService>();
builder.Services.AddScoped<IReviewService, ReviewsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
