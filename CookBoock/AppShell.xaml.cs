using CookBoock.View;
namespace CookBoock;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		CurrentItem = (ShellItem)Home;
        Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
        Routing.RegisterRoute(nameof(RecipePageApi), typeof(RecipePageApi));
        Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
        Routing.RegisterRoute(nameof(User), typeof(User));
        Application.Current.UserAppTheme = AppTheme.Light;
    }
}
