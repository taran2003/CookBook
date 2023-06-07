using CookBoock.Models;
using CookBoock.Data;

namespace CookBoock;

public partial class AddPage : ContentPage
{
    private RecipeDB Db;

	public AddPage()
	{
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
    }

    private void AddIngridient(object sender, EventArgs e)
    {
		Ingridients.Add(new Entry());
    }

    private void RemoveIngridient(object sender, EventArgs e)
    {
        if(Ingridients.Count > 1)
        {
            Ingridients.Remove(Ingridients[Ingridients.Count() - 1]);
        }
    }

    private async void Save(object sender, EventArgs e)
    {
        string[] ingridients = new string[Ingridients.Count - 1];
        for (int i = 0; i < Ingridients.Count; i++)
        {
            if(i == 0) { continue; }
            var buf = (Entry)Ingridients[i];
            ingridients[i - 1] = buf.Text;
        }
        Recipe recipe = new Recipe(Name.Text, ingridients, CookingProcess.Text);
        Db.Recipes.Insert(recipe);
        Db.Db.Dispose();
        await Shell.Current.GoToAsync("..");
    }
}