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
        BindingContext = new RecipePageViewModel(ItemId,Shell.Current);
    }

    public string ItemId
    {
        get;
        set;
    }

    private async void GoAddPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"AddPage?ItemId={ItemId}");
    }
}