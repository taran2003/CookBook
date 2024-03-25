using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CookBoock.ViewModel
{
    class FavoritesViewModel : INotifyPropertyChanged
    {
        public ICommand ColoseDb { get; set; }
        public ICommand DeleteItem { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SelectRecipeCommand { get; }
        public ICommand RefreshCommand { get; }
        private RecipeDB Db;
        private ObservableCollection<Recipe> recipesList = new();
        public ObservableCollection<Recipe> RecipesList
        {
            get => recipesList;
            set => SetProperty(ref recipesList, value);
        }

        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref  isRefreshing, value);            
        }

        private IList<Recipe> _allRecipes;
            
        public FavoritesViewModel()
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            ColoseDb = new Command(() =>
            {
                Db.Close();
            });
            //Db.ClearAll();
            DeleteItem = new Command<Recipe>(Delete);
            SearchCommand = new Command<string>(Search);
            RefreshCommand = new Command(refresh);
            SelectRecipeCommand = new Command<Recipe>(async (r) => await SelectRecipeAsync(r));
        }

        private bool _isInitialized;

        public async Task InitAsync()
        {
            if (_isInitialized)
            {
                return;
            }

            var recipes = new List<Recipe>();
            await Task.Run(() => recipes = Db.GetAll());

            _allRecipes = recipes;

            foreach (var item in recipes)
            {
                await MainThread.InvokeOnMainThreadAsync(()=>RecipesList.Add(item));
            }
            Db.Close() ;
            _isInitialized = true;
        }

        private void refresh()
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            var recipes = new List<Recipe>();
            recipes = Db.GetAll();
            _allRecipes = recipes;
            recipesList.Clear();
            foreach (var item in recipes)
            {
                RecipesList.Add(item);
            }
            IsRefreshing = false;
        }

        private void Search(string text)
        {
            RecipesList.Clear();
            var results = _allRecipes.Where(recipe=>recipe.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var item in results)
            {
                RecipesList.Add(item);
            }
        
        }

        private void Delete(Recipe Item)
        {
            RecipesList.Remove(Item);
            OnPropertyChanged();
            Db.DeleteById(Item.Id);
        }

        private async Task SelectRecipeAsync(Recipe recipe)
        {
            await Shell.Current.GoToAsync($"RecipePage?ItemId={recipe.Id.ToString()}");
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
