using CaWorkshop.Infrastructure.Data;
using CaWorkshop.Application;
using CaWorkshop.Infrastructure;
using CaWorkshop.WebUI.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddRazorPages();

builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "CaWorkshop API";
});
builder.Services.AddControllersWithViews(options => options.Filters.Add(new ApiExceptionFilterAttribute()));

var app = builder.Build();

// Initialise and seed the database on start-up
using (var scope = app.Services.CreateScope())
{
    try
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        initialiser.Initialise();
        initialiser.Seed();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialisation");

        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseOpenApi();
app.UseSwaggerUi3();

app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();