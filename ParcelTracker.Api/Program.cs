using ParcelTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<GoogleSheetsService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
