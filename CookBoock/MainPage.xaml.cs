using CookBoock.Models;
using CookBoock.ViewModel;

namespace CookBoock;

public partial class MainPage : ContentPage
{
	MainPageViewModel viewModel;

	public MainPage()
	{
		InitializeComponent();
        BindingContext = viewModel = new MainPageViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ = viewModel.InitAsync();
    }

    private void RecipeList_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        viewModel.RemainingItemseachedCommand.Execute(null);
    }
}

