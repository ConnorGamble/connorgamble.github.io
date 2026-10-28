using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Portfolio.Site;

/// <summary>
/// Everything in Content/Data: site details, sections and experience.
/// </summary>
public class SiteData
{
    public SiteInfo Info { get; init; } = new();
    public List<Section> Sections { get; init; } = [];
    public ExperienceData Experience { get; init; } = new();

    public Section? FindSection(string? key) =>
        Sections.FirstOrDefault(s => string.Equals(s.Key, key, StringComparison.OrdinalIgnoreCase));

    public Section? FindSectionBySlug(string? slug) =>
        Sections.FirstOrDefault(s => string.Equals(s.Slug, slug, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Section> NavSections => Sections.Where(s => s.Nav);

    public static readonly IDeserializer Yaml = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    public static SiteData Load(string contentRoot)
    {
        var dataFolder = Path.Combine(contentRoot, "Content", "Data");

        T Read<T>(string file) where T : new()
        {
            var path = Path.Combine(dataFolder, file);
            return File.Exists(path) ? Yaml.Deserialize<T>(File.ReadAllText(path)) ?? new T() : new T();
        }

        return new SiteData
        {
            Info = Read<SiteInfo>("site.yml"),
            Sections = Read<List<Section>>("sections.yml"),
            Experience = Read<ExperienceData>("experience.yml"),
        };
    }
}

public class SiteInfo
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Author { get; set; } = "";
    public string ShortName { get; set; } = "";
    public string Tagline { get; set; } = "";
    public string Avatar { get; set; } = "";
    public string AvatarAlt { get; set; } = "";
    public string GitHub { get; set; } = "";
    public List<string> Skills { get; set; } = [];
}

public class Section
{
    /// <summary>Matches a post's <c>section:</c> front matter (or its first tag).</summary>
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    private string? _slug;
    public string Slug { get => _slug ?? Key.ToLowerInvariant(); set => _slug = value; }
    public bool Nav { get; set; }
    /// <summary>Show only the latest N posts, with a link to the rest.</summary>
    public int? Limit { get; set; }
    /// <summary>Heading used when this section is one of several lists on a page.</summary>
    public string? ListHeading { get; set; }
    public string? ListDescription { get; set; }
    /// <summary>Other section keys whose posts are listed underneath this one.</summary>
    public List<string> Include { get; set; } = [];
}

public class ExperienceData
{
    public List<ExperienceEntry> Roles { get; set; } = [];
    public List<ExperienceEntry> Education { get; set; } = [];
}

public class ExperienceEntry
{
    public string Title { get; set; } = "";
    public string Org { get; set; } = "";
    public string Period { get; set; } = "";
    public bool Current { get; set; }
    public List<string> Highlights { get; set; } = [];
    /// <summary>Optional markdown shown under the highlights.</summary>
    public string? Note { get; set; }
}
