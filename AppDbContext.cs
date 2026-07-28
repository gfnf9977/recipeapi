using Microsoft.EntityFrameworkCore;

namespace RecipeApi;

public class AppDbContext : DbContext
{
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<VideoLink> VideoLinks => Set<VideoLink>();
    public DbSet<RecipeStep> Steps => Set<RecipeStep>();
    public DbSet<RecipeTip> Tips => Set<RecipeTip>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=recipes.db");
    }
}