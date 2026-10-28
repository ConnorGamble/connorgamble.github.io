using BlazorStatic;
using Portfolio.Components;
using Portfolio.Site;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

// Section pages have a route parameter, so BlazorStatic needs telling which ones to generate.
var sections = SiteData.Load(builder.Environment.ContentRootPath).Sections;

builder.Services.AddBlazorStaticService(opt =>
{
    opt.OutputFolderPath = "output";
    opt.SitemapOutputFolderPath = "output/sitemap";
    opt.MarkdownPipeline = PageContentService.Pipeline;

    foreach (var section in sections)
    {
        opt.PagesToGenerate.Add(new PageToGenerate(section.Slug, Path.Combine(section.Slug, "index.html")));
        if (section.Limit.HasValue)
            opt.PagesToGenerate.Add(new PageToGenerate($"{section.Slug}/all", Path.Combine(section.Slug, "all", "index.html")));
    }
}
).AddBlazorStaticContentService<PostFrontMatter>();

// Data files are re-read per request so edits show up without a restart while running locally.
builder.Services.AddScoped(sp => SiteData.Load(sp.GetRequiredService<IWebHostEnvironment>().ContentRootPath));
builder.Services.AddSingleton<PageContentService>();

builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
// Route after static files, otherwise the /{Slug} section route swallows root files like /Tickle.png.
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.UseBlazorStaticGenerator(shutdownApp: !app.Environment.IsDevelopment());

app.Run();
