namespace CookBoock;

public partial class User : ContentPage
{
	public User()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new ViewModel.User();
    }
}