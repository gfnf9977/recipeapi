using Microsoft.EntityFrameworkCore;
using RecipeApi;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/recipes", async (AppDbContext db) =>
{
    return await db.Recipes
        .Include(r => r.Ingredients)
        .Include(r => r.VideoLinks)
        .Include(r => r.Steps)
        .Include(r => r.Tips)
        .OrderByDescending(r => r.CreatedAt)
        .ToListAsync();
});

app.MapPost("/api/recipes", async (AppDbContext db, Recipe newRecipe) =>
{
    foreach (var video in newRecipe.VideoLinks)
    {
        video.ThumbnailUrl = await FetchThumbnailAsync(video.Url);
    }

    if (string.IsNullOrEmpty(newRecipe.PhotoUrl) && newRecipe.VideoLinks.Count > 0)
    {
        newRecipe.PhotoUrl = newRecipe.VideoLinks[0].ThumbnailUrl;
    }

    db.Recipes.Add(newRecipe);
    await db.SaveChangesAsync();
    return Results.Ok(newRecipe);
});

app.MapPut("/api/recipes/{id}", async (int id, AppDbContext db, Recipe updatedRecipe) =>
{
    var recipe = await db.Recipes
        .Include(r => r.Ingredients).Include(r => r.VideoLinks)
        .Include(r => r.Steps).Include(r => r.Tips)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (recipe == null) return Results.NotFound();

    foreach (var video in updatedRecipe.VideoLinks)
    {
        video.ThumbnailUrl = await FetchThumbnailAsync(video.Url);
    }

    if (string.IsNullOrEmpty(updatedRecipe.PhotoUrl) && updatedRecipe.VideoLinks.Count > 0)
    {
        updatedRecipe.PhotoUrl = updatedRecipe.VideoLinks[0].ThumbnailUrl;
    }

    recipe.Title = updatedRecipe.Title;
    recipe.PhotoUrl = updatedRecipe.PhotoUrl;

    db.Ingredients.RemoveRange(recipe.Ingredients);
    db.VideoLinks.RemoveRange(recipe.VideoLinks);
    db.Steps.RemoveRange(recipe.Steps);
    db.Tips.RemoveRange(recipe.Tips);

    updatedRecipe.Ingredients.ForEach(i => i.Id = 0);
    updatedRecipe.VideoLinks.ForEach(v => v.Id = 0);
    updatedRecipe.Steps.ForEach(s => s.Id = 0);
    updatedRecipe.Tips.ForEach(t => t.Id = 0);

    recipe.Ingredients = updatedRecipe.Ingredients;
    recipe.VideoLinks = updatedRecipe.VideoLinks;
    recipe.Steps = updatedRecipe.Steps;
    recipe.Tips = updatedRecipe.Tips;

    await db.SaveChangesAsync();
    return Results.Ok(recipe);
});

app.MapDelete("/api/recipes/{id}", async (int id, AppDbContext db) =>
{
    var recipe = await db.Recipes.FindAsync(id);
    if (recipe != null)
    {
        db.Recipes.Remove(recipe);
        await db.SaveChangesAsync();
    }
    return Results.Ok();
});

app.Run();

async Task<string?> FetchThumbnailAsync(string url)
{
    try
    {
        using var client = new HttpClient();

        if (url.Contains("tiktok.com"))
        {
            var oembedUrl = $"https://www.tiktok.com/oembed?url={url}";
            var jsonStr = await client.GetStringAsync(oembedUrl);
            using var doc = JsonDocument.Parse(jsonStr);
            if (doc.RootElement.TryGetProperty("thumbnail_url", out var thumb))
            {
                return thumb.GetString();
            }
        }
        else if (url.Contains("youtube.com") || url.Contains("youtu.be"))
        {
            var oembedUrl = $"https://www.youtube.com/oembed?url={url}&format=json";
            var jsonStr = await client.GetStringAsync(oembedUrl);
            using var doc = JsonDocument.Parse(jsonStr);
            if (doc.RootElement.TryGetProperty("thumbnail_url", out var thumb))
            {
                return thumb.GetString();
            }
        }
        else
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            var html = await client.GetStringAsync(url);
            var match = System.Text.RegularExpressions.Regex.Match(html, @"<meta\s+property=""og:image""\s+content=""([^""]+)""");
            if (match.Success)
            {
                return match.Groups[1].Value.Replace("&amp;", "&");
            }
        }
    }
    catch
    {
    }
    return null;
}