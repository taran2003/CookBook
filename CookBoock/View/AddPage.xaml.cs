using CookBoock.Models;
using CookBoock.Data;

namespace CookBoock;

public partial class AddPage : ContentPage
{
    private AddPageViewModel model;

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