using CookBoock.ViewModel;

namespace CookBoock;

public partial class Favorites : ContentPage
{
    private FavoritesViewModel viewModel;

    public Favorites()
	{
		InitializeComponent();

        BindingContext = viewModel = new FavoritesViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ = viewModel.InitAsync();
    }

    private async void Add(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPage());
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        viewModel.SearchCommand?.Execute(e.NewTextValue);
    }
}