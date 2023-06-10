using CookBoock.Data;
using CookBoock.Models;
using CookBoock.ViewModel;
using Microsoft.Maui.Controls;


namespace CookBoock;

public partial class Favorites : ContentPage
{
    public Favorites()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new FavoritesViewModel();
    }

    private async void Add(object sender, EventArgs e)
    {
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