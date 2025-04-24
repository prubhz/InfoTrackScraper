using InfoTrackScraper.Services;
using InfoTrackScraper.Validators;

var builder = WebApplication.CreateBuilder(args);

// Define CORS policy name
const string AllowSpecificOrigins = "_allowSpecificOrigins";

// Add services to the container.

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200") // Allow Angular dev server
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddHttpClient();
builder.Services.AddTransient<HardCodedTest>();
builder.Services.AddTransient<GoogleSearchService>();
builder.Services.AddTransient<CustomSearchApiService>();
builder.Services.AddTransient<ScrapeRequestValidator>();
builder.Services.AddTransient<NativeBrowserSearchService>();
builder.Services.AddTransient<WebView2SearchService>();

// Register IConfiguration for dependency injection
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS middleware
app.UseCors(AllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
