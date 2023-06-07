using CookBoock.Data;
using CookBoock.Models;

namespace CookBoock;

[QueryProperty(nameof(ItemId), "ItemId")]
public partial class RecipePage : ContentPage
{
    private RecipeDB Db;
    private int id;

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
        for (int i = 0; i < recipe.Ingridients.Count; i++)
        {
            label = new Label();
            label.Text = recipe.Ingridients[i].Ingridient;
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
            id = Convert.ToInt32(itemId);
            var recipe = Db.FindeById(id);
            SetContent(recipe);
            BindingContext = recipe;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to load recipe.");
        }
    }

    private async void Delete(object sender, EventArgs e)
    {
        Db.DeleteById(id);
        await Shell.Current.GoToAsync("..");
    }

    private void Rewrite(object sender, EventArgs e)
    {
        
    }
}