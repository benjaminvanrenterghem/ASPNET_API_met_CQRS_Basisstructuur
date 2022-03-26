using API.Extensions;
using Domain.Static;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region custom
// Alternatief appsettings.json, arg= builder.Configuration["ConnectionStrings:Main"]
builder.Services.ConfigureServicelayer(ApiConfig.ConnectionString);

var app = builder.Build();
app.UseCors("OpenPolicy");
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();