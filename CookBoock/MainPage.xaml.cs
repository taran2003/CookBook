using CookBoock.Models;
using CookBoock.ViewModel;

namespace CookBoock;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();
	}

    private async void RecipeListSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            Recipe recipe = (Recipe)e.CurrentSelection.FirstOrDefault();
            await Shell.Current.GoToAsync($"RecipePageApi?ItemId={recipe.Id.ToString()}");
        }
    }
}

