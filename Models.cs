namespace RecipeApi;

public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<VideoLink> VideoLinks { get; set; } = new();
    public List<RecipeStep> Steps { get; set; } = new();
    public List<RecipeTip> Tips { get; set; } = new();
}

public class Ingredient
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsMandatory { get; set; } = true;
}

public class VideoLink
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
}

public class RecipeStep
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class RecipeTip
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string Description { get; set; } = string.Empty;
}