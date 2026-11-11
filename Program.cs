using BusinessReportingAI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Registracija HttpClient-a i servisa
builder.Services.AddHttpClient<IOllamaService, OllamaService>();
builder.Services.AddSingleton<ISqlValidator, SqlValidator>();
builder.Services.AddScoped<IDbReportService, DbReportService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();