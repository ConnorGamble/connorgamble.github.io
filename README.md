Connor G's portfolio
==========

The code for my portfolio, live at [connorgamble.github.io](https://connorgamble.github.io).

It's a .NET 8 Blazor app that [BlazorStatic](https://blazorstatic.net/) turns into plain HTML. Pushing to `main` runs the GitHub Action, which generates the site into `output/` and publishes it to GitHub Pages.

## Running it locally

```bash
dotnet watch --launch-profile http
```

Then open http://localhost:5250. Markdown and data file changes show up on refresh.

To generate the static site the same way CI does:

```bash
dotnet run -c Release --no-launch-profile
```

The result lands in `output/`.

## Where things live

| Path | What it is |
| --- | --- |
| `Content/Blog/*.md` | Posts (projects, games, personal, reviews) |
| `Content/Blog/media/` | Images used by posts |
| `Content/Pages/*.md` | Intro text at the top of a page, one file per page |
| `Content/Data/site.yml` | Site title, description, tagline, avatar, GitHub link, skills |
| `Content/Data/sections.yml` | The sections (list pages) and which appear in the nav |
| `Content/Data/experience.yml` | Roles and education on the Experience page |
| `Components/` | Blazor components (layout, pages, cards) |
| `wwwroot/css/site.css` | All styling; colours are tokens at the top of the file |
| `wwwroot/js/site.js` | Mobile menu, scroll reveal, card glow, code highlighting |

## Templates

- `Content/Blog/PostTemplate.md`: a general post with every front matter option commented.
- `Content/Blog/GameReviewTemplate.md`: the DUCK layout for game reviews.
- `Content/Pages/_template.md`: a page intro.

Both post templates are `draft: true` and underscore page files are ignored, so none of them get published. Copy one, rename it and fill it in.

## Adding a post

Copy `Content/Blog/PostTemplate.md`, or create `Content/Blog/My Post.md` from scratch:

```markdown
---
title: My Post
lead: One line shown on the card and under the title
published: 2026-10-28
tags: [ Projects, C#, Blazor ]
---
Post content in markdown. Images go in `Content/Blog/media/` and are linked as ![alt](media/MyPost/Screenshot.png).
A YouTube link written as an image embeds the video: ![Trailer](https://youtu.be/abc123)
```

The post goes in the section whose `key` matches its first tag (`Projects` above). Section tags are hidden from the tag chips, so only the other tags show.

Optional front matter:

| Field | Does |
| --- | --- |
| `section: Personal` | Picks the section explicitly instead of using the first tag |
| `cover: media/MyPost/Cover.png` | Card image (defaults to the first image in the post) |
| `pageTitle: Something else` | Browser tab title (defaults to the post title) |
| `draft: true` | Not generated. `GameReviewTemplate.md` uses this so it stays a template |

The post is published at `/blog/{file name without .md}`.

## Adding a section

1. Add an entry to `Content/Data/sections.yml`:

   ```yaml
   - key: Music
     name: Music
     nav: true
   ```

   The file's header comment lists the other options (`slug`, `limit`, `include`, list headings).
2. Optionally add `Content/Pages/music.md` for the intro text at the top of the page.
3. Tag posts with `Music` as the first tag (or set `section: Music`).

The page is generated at `/music`. Leave `nav` off to keep it out of the menu while it's a work in progress (like Game Reviews).

## Page intros

Any page can have an intro in `Content/Pages/{slug}.md`: `home.md`, `experience.md`, or a section slug such as `personal.md`.

```markdown
---
title: Yo, I'm Connor     # page heading
accent: Connor            # optional, picked out in the accent colour
pageTitle: Connor Gamble  # optional browser tab title (defaults to the site title)
description: ...          # optional meta description
---
Intro text in markdown.
```

## Experience

Edit `Content/Data/experience.yml`. Roles with `current: true` show under "Current Role", the rest under "Previous Roles". `note` is optional markdown shown under the highlights.
