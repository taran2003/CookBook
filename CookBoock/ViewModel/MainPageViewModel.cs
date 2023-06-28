using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CookBoock.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ICommand SelectRecipeCommand { get; }
        public ICommand RemainingItemseachedCommand { get; }

        private ObservableCollection<Recipe> recipesList = new();
        public ObservableCollection<Recipe> RecipesList
        {
            get => recipesList;
            set
            {
                SetProperty(ref recipesList, value);
            }
        }

        public MainPageViewModel()
        {
            SelectRecipeCommand = new Command<Recipe>(async(r)=>await SelectRecipeAsync(r));
            RemainingItemseachedCommand = new Command(async()=>await UpdateList());
        }

        public bool IsDataLoading { get; private set; } = false;

        public async Task UpdateList()
        {
            if (!_isInitialized || IsDataLoading) { return; }

            await MainThread.InvokeOnMainThreadAsync(() => IsDataLoading = true);

            var recipes = new List<Recipe>();
            await Task.Run(() => recipes = RecipeApi.GetRecipes(_from, _to));

            foreach (var item in recipes)
            {
                _from += _to;
                await MainThread.InvokeOnMainThreadAsync(() => RecipesList.Add(item));
            }
            await MainThread.InvokeOnMainThreadAsync(() => IsDataLoading = false);

        }

        private bool _isInitialized;
        private int _from = 0;
        private const int _to = 5;

        public async Task InitAsync()
        {
            if (_isInitialized) { return; }

            var recipes = new List<Recipe>();
            await Task.Run(() => recipes = RecipeApi.GetRecipes(_from, _to));

            foreach (var item in recipes)
            {
                _from += _to;
                await MainThread.InvokeOnMainThreadAsync(() => RecipesList.Add(item));
            }

            _isInitialized = true;
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

        private async Task SelectRecipeAsync(Recipe recipe)
        {
            await Shell.Current.GoToAsync($"RecipePageApi?ItemId={recipe.Id.ToString()}");
        }
    }
}
