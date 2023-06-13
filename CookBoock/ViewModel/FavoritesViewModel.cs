using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static Android.Content.ClipData;

namespace CookBoock.ViewModel
{
    class FavoritesViewModel : INotifyPropertyChanged
    {
        public ICommand ColoseDb { get; set; }
        public ICommand DeleteItem { get; set; }
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
            
        public FavoritesViewModel()
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            RecipesList = new ObservableCollection<Recipe>(Db.GetAll());
            ColoseDb = new Command(() =>
            {
                Db.Close();
            });
            DeleteItem = new Command<Recipe>((Recipe Item) =>
            {
                RecipesList.Remove(Item);
                OnPropertyChanged();
                Db.DeleteById(Item.Id);
            });
        }

        private void Delete(Recipe Item)
        {
            RecipesList.Remove(RecipesList[Item.Id]);
            OnPropertyChanged();
            Db.DeleteById(Item.Id);
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
