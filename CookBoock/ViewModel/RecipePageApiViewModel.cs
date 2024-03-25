using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CookBoock.Data;
using CookBoock.Models;
using CookBoock.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CookBoock.ViewModel
{
    class RecipePageApiViewModel : INotifyPropertyChanged
    {
        private Recipe recipe;
        public ICommand ToFavorites { get; set; }
        private string name;
        public string Name
        {
            get => recipe?.Name ?? "loading...";
            set
            {
                SetProperty(ref name, value);
            }
        }
        private ObservableCollection<Ingridients> ingridients = new();
        public ObservableCollection<Ingridients> Ingridients
        {
            get => ingridients;
            set
            {
                SetProperty(ref ingridients, value);
            }
        }
        private string cookingProcess;
        public string CookingProcess
        {
            get => cookingProcess;
            set
            {
                SetProperty(ref cookingProcess, value);
            }
        }
        private Microsoft.Maui.Graphics.IImage image;
        public Microsoft.Maui.Graphics.IImage Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
        private float height;
        public float Height
        {
            get => height;
            set { SetProperty(ref height, value); }
        }
        private float width;
        public float Width
        {
            get => width;
            set { SetProperty(ref width, value); }
        }

        public RecipePageApiViewModel(string id)
        {
            ToFavorites = new Command(AddToFavorites);
            this.id = id;
        }

        private bool _isInitialized;
        private readonly string id;

        public async Task InitAsync()
        {
            if (_isInitialized) { return; }

            await Task.Run(() =>
            {
                recipe = RecipeApi.GetRecipe(id);
                recipe.Image = ImageGeter.GetImageFromUrl(recipe.ImageUrl);
                recipe.IsLoad = true;
            });

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Name = recipe.Name;
                Ingridients = recipe.Ingridients;
                Image = recipe.Image;
                Height = Image.Height;
                Width = Image.Width;
            });


            _isInitialized = true;
        }

        public async void AddToFavorites()
        {
            RecipeDB dB = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            var recipe = new Recipe(this.recipe);
            dB.Add(recipe);
            dB.Close();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = Constants.Texts.ToastAddToFavorites;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

        bool SetProperty<T>(ref T storeg, T value, [CallerMemberName] string propertyNmae = null)
        {
            if (Object.ReferenceEquals(storeg, value))
                return false;
            storeg = value;
            OnPropertiChanged(propertyNmae);
            return true;
        }

        protected void OnPropertiChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
