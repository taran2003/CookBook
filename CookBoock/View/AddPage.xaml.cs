using CookBoock.Models;
using CookBoock.Data;
using CookBoock.ViewModel;

namespace CookBoock;

public partial class AddPage : ContentPage
{
	public AddPage()
	{
        InitializeComponent();
        BindingContext = new AddPageViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void GoBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}