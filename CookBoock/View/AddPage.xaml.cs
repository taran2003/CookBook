using CookBoock.ViewModel;
namespace CookBoock;

[QueryProperty(nameof(ItemId), "ItemId")]
public partial class AddPage : ContentPage
{
	public AddPage()
	{
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(ItemId==null) 
        {
            BindingContext = new AddPageViewModel();
        }
        else
        {
            BindingContext = new AddPageViewModel(ItemId);
        }
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
}