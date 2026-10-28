using Markdig;

namespace Portfolio.Site;

/// <summary>Front matter for page intros in Content/Pages.</summary>
public class PageFrontMatter
{
    /// <summary>The page heading.</summary>
    public string? Title { get; set; }
    /// <summary>A word or phrase in the title to pick out in the accent colour.</summary>
    public string? Accent { get; set; }
    /// <summary>Overrides the browser tab title.</summary>
    public string? PageTitle { get; set; }
    /// <summary>Overrides the meta description.</summary>
    public string? Description { get; set; }
}

public record PageContent(PageFrontMatter FrontMatter, string Html);

/// <summary>
/// Reads Content/Pages/{name}.md. Files are read on every request so edits show up straight away while running locally.
/// </summary>
public class PageContentService(IWebHostEnvironment env)
{
    public static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseYamlFrontMatter()
        .Build();

    public PageContent? Get(string name)
    {
        // Underscore files (like _template.md) are never used.
        if (name.StartsWith('_'))
            return null;

        var path = Path.Combine(env.ContentRootPath, "Content", "Pages", $"{name}.md");
        if (!File.Exists(path))
            return null;

        var text = File.ReadAllText(path).TrimStart('﻿');
        var frontMatter = new PageFrontMatter();

        if (text.StartsWith("---"))
        {
            var end = text.IndexOf("\n---", 3, StringComparison.Ordinal);
            if (end > 0)
            {
                frontMatter = SiteData.Yaml.Deserialize<PageFrontMatter>(text[3..end]) ?? frontMatter;
                var bodyStart = text.IndexOf('\n', end + 1);
                text = bodyStart < 0 ? "" : text[(bodyStart + 1)..];
            }
        }

        return new PageContent(frontMatter, Render(text));
    }

    public static string Render(string markdown) => Markdown.ToHtml(markdown, Pipeline);
}
