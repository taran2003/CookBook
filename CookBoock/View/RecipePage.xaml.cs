using CookBoock.ViewModel;
namespace CookBoock;

[QueryProperty(nameof(ItemId), "ItemId")]
public partial class RecipePage : ContentPage
{

    RecipePageViewModel viewModel;

    public RecipePage()
	{
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(_id != null)
        {
            BindingContext = viewModel = new RecipePageViewModel(ItemId);
        }
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
                BindingContext = viewModel = new RecipePageViewModel(ItemId);
            }
        }
    }

    private async void GoAddPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"AddPage?ItemId={ItemId}");
    }
}