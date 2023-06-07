using CookBoock.Data;
using CookBoock.Models;

namespace CookBoock;

[QueryProperty(nameof(ItemId), "ItemId")]
public partial class RecipePage : ContentPage
{
    private RecipeDB Db;

    public RecipePage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    public string ItemId
    {
        set
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            LoadRecipe(value);
        }
    }

    private void SetContent(Recipe recipe)
    {
        Label label = new Label();
        for (int i = 0; i < recipe.Ingridients.Length; i++)
        {
            label = new Label();
            label.Text = recipe.Ingridients[i];
            MainContent.Add(label);
        }
        label = new Label(); 
        label.Text = recipe.CookingProcess;
        MainContent.Add(label);

    }

    void LoadRecipe(string itemId)
    {
        try
        {
            int id = Convert.ToInt32(itemId);
            var recipe = Db.Recipes.FindOne(x => x.Id == id);
            SetContent(recipe);
            BindingContext = recipe;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to load recipe.");
        }
    }
}