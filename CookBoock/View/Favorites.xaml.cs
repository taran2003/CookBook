using CookBoock.Data;
using CookBoock.Models;
using Microsoft.Maui.Controls;


namespace CookBoock;

public partial class Favorites : ContentPage
{
    private RecipeDB Db;

    public Favorites()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
        RecipeList.ItemsSource = Db.GetAll();
    }

    private async void Add(object sender, EventArgs e)
    {
        Db.Close();
		await Navigation.PushAsync(new AddPage());
    }

    private async void RecipeListSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            Recipe recipe = (Recipe)e.CurrentSelection.FirstOrDefault();
            await Shell.Current.GoToAsync($"RecipePage?ItemId={recipe.Id.ToString()}");
        }
    }
}