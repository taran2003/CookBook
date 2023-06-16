using CookBoock.Models;
using CookBoock.ViewModel;

namespace CookBoock;

public partial class ShopList : ContentPage
{
	public ShopList()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new ShopListViewModel();
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