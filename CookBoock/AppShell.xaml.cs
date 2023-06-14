﻿namespace CookBoock;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		CurrentItem = (ShellItem)Home;
        Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
        Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
        Application.Current.UserAppTheme = AppTheme.Light;
    }
}
