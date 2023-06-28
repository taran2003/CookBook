using CookBoock.ViewModel;

namespace CookBoock.View;

[QueryProperty(nameof(ItemId), "ItemId")]
public partial class RecipePageApi : ContentPage
{
    RecipePageApiViewModel viewModel;

	public RecipePageApi()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.InitAsync();    
    }

    private string _id;
    public string ItemId
    {
        get => _id;
        set
        {
            _id = value;

            if (BindingContext == default)
            {
                BindingContext = viewModel = new RecipePageApiViewModel(ItemId);
            }
        }
    }
}