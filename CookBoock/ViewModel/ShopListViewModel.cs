using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CookBoock.ViewModel
{
    class ShopListViewModel : INotifyPropertyChanged
    {
        public ICommand ColoseDb { get; }
        public ICommand DeleteItem { get; }
        public ICommand SelectRecipeCommand { get; }
        private RecipeDB Db;
        private ObservableCollection<Recipe> recipesList;
        public ObservableCollection<Recipe> RecipesList
        {
            get => recipesList;
            set
            {
                SetProperty(ref recipesList, value);
            }
        }

        public ShopListViewModel()
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            RecipesList = new ObservableCollection<Recipe>(Db.FindeAllByCart());
            ColoseDb = new Command(() =>
            {
                Db.Close();
            });
            DeleteItem = new Command<Recipe>(Delete);
            SelectRecipeCommand = new Command<Recipe>(async (r) => await SelectRecipeAsync(r));
        }

        private async Task SelectRecipeAsync(Recipe recipe)
        {
            await Shell.Current.GoToAsync($"RecipePage?ItemId={recipe.Id.ToString()}");
        }

        private void Delete(Recipe Item)
        {
            Recipe recipe = Db.FindeById(Item.Id);
            recipe.RemoveFromCart();
            RecipesList.Remove(Item);
            Db.Update(recipe);
        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
