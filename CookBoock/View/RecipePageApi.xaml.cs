using CookBoock.ViewModel;

namespace CookBoock.View;

[QueryProperty(nameof(ItemId), "ItemId")]
public partial class RecipePageApi : ContentPage
{
	public RecipePageApi()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new RecipePageApiViewModel(ItemId);
    }

    public string ItemId
    {
        get;
        set;
    }
}