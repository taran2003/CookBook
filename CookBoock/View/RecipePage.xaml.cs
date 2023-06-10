using CookBoock.Data;
using CookBoock.Models;
using CookBoock.ViewModel;

namespace CookBoock;

[QueryProperty(nameof(ItemId), "ItemId")]
public partial class RecipePage : ContentPage
{
    public RecipePage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new RecipePageViewModel(ItemId);
    }

    public string ItemId
    {
        get;
        set;
    }

    private async void GoBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void Rewrite(object sender, EventArgs e)
    {
        
    }
}