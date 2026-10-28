using BlazorStatic;
using YamlDotNet.Serialization;

namespace Portfolio.Site;

/// <summary>
/// Front matter for posts in Content/Blog. Only title, lead, published and tags are required;
/// everything else is optional so older posts keep working untouched.
/// </summary>
public class PostFrontMatter : IFrontMatter, IFrontMatterWithTags
{
    public string Title { get; set; } = "";
    public string Lead { get; set; } = "";
    public DateTime Published { get; set; }
    public List<string> Tags { get; set; } = [];

    /// <summary>Section key (see Content/Data/sections.yml). Falls back to the first tag when missing.</summary>
    public string? Section { get; set; }

    /// <summary>Card image for post lists. Falls back to the first image in the post.</summary>
    public string? Cover { get; set; }

    /// <summary>Overrides the browser tab title. Falls back to the post title.</summary>
    public string? PageTitle { get; set; }

    /// <summary>Drafts are not generated.</summary>
    [YamlMember(Alias = "draft")]
    public bool IsDraft { get; set; }

    [YamlIgnore]
    public AdditionalInfo? AdditionalInfo => new() { LastMod = Published };
}
