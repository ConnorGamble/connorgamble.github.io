using System.Text.RegularExpressions;
using BlazorStatic;

namespace Portfolio.Site;

public static partial class PostExtensions
{
    public static string? SectionKey(this Post<PostFrontMatter> post) =>
        post.FrontMatter.Section ?? post.FrontMatter.Tags.FirstOrDefault();

    public static IEnumerable<Post<PostFrontMatter>> InSection(this IEnumerable<Post<PostFrontMatter>> posts, string key) =>
        posts.Where(p => string.Equals(p.SectionKey(), key, StringComparison.OrdinalIgnoreCase))
             .OrderByDescending(p => p.FrontMatter.Published);

    /// <summary>Tags worth showing as chips: section keys are left out since the section is already shown.</summary>
    public static IEnumerable<Tag> DisplayTags(this Post<PostFrontMatter> post, SiteData site) =>
        post.Tags.Where(t => site.FindSection(t.Name) is null);

    /// <summary>The front matter cover, or the first image in the post.</summary>
    public static string? CoverImage(this Post<PostFrontMatter> post)
    {
        var cover = post.FrontMatter.Cover;
        if (!string.IsNullOrWhiteSpace(cover))
            return cover.StartsWith("media/") ? $"Content/Blog/{cover}" : cover;

        var match = FirstImage().Match(post.HtmlContent ?? "");
        return match.Success ? match.Groups[1].Value : null;
    }

    public static int ReadingMinutes(this Post<PostFrontMatter> post)
    {
        var words = HtmlTag().Replace(post.HtmlContent ?? "", " ")
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(1, (int)Math.Round(words / 220.0));
    }

    [GeneratedRegex("<img[^>]+src=\"([^\"]+)\"", RegexOptions.IgnoreCase)]
    private static partial Regex FirstImage();

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex HtmlTag();
}
