using Chu.Bank.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureWebApi();
builder.Services.ConfigureBusinessServices();
builder.Services.ConfigureDataAccess(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();