using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CookBoock.ViewModel
{
    class RecipePageApiViewModel
    {
        private int Id;
        private Recipe recipe;
        public ICommand ToFavorites { get; set; }
        private string name;
        public string Name
        {
            get => recipe.Name;
            set
            {
                SetProperty(ref name, value);
            }
        }
        private ObservableCollection<Ingridients> ingridients;
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
        private ImageSource image;
        public ImageSource Image
        {
            get => image;
            set { SetProperty(ref image, value); }
        }

        public RecipePageApiViewModel(string id)
        {
            Id = Convert.ToInt32(id);
            recipe = RecipeApi.GetRecipe(id);
            name = recipe.Name;
            Ingridients = recipe.Ingridients;
            cookingProcess = recipe.CookingProcess;
            image = recipe.Image;
            ToFavorites = new Command(AddToFavorites);
        }

        public async void AddToFavorites()
        {

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
