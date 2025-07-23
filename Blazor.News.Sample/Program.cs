using Blazor.News;
using Blazor.News.Data;
using Blazor.News.Data.Seed;
using Blazor.News.Sample.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();


// Hier roep je de extensiemethode aan vanuit je RCL
builder.Services.AddSiteNews(builder.Configuration);


var app = builder.Build();


// **Belangrijk: Database migratie en seeding bij opstarten**
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Migreer de NewsDBContext
        var newsContext = services.GetRequiredService<NewsDBContext>();
        newsContext.Database.Migrate();

        NieuwsCategorySeeder.SeedCategories(newsContext);
        NieuwsArtikelSeeder.SeedNieuwsArtikelen(newsContext);
        // Migreer ook de parent DbContext indien aanwezig
        // var parentContext = services.GetRequiredService<ApplicationDbContext>();
        // parentContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        // Optioneel: gooi de exception opnieuw of handel deze af zoals gewenst
    }
}





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
app.Run();
