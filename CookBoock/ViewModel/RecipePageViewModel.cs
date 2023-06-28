using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CookBoock.Helpers;
using CookBoock.View;

namespace CookBoock.ViewModel
{
    class RecipePageViewModel : INotifyPropertyChanged
    {
        private int Id;
        private RecipeDB Db;
        private Recipe recipe;
        public ICommand Delete {get; set;}
        public ICommand ToCart { get; set;}
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
        private Microsoft.Maui.Graphics.IImage image;
        public Microsoft.Maui.Graphics.IImage Image
        {
            get => image;
            set { SetProperty(ref image, value); }
        }

        public RecipePageViewModel(string id)
        {
            Id = Convert.ToInt32(id);
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            recipe = Db.FindeById(Id);
            name = recipe.Name;
            Ingridients = recipe.Ingridients;
            cookingProcess = recipe.CookingProcess;
            image = Db.GetImage(recipe.FileId);
            Delete = new Command(Delite);
            ToCart = new Command(AddToCart);
        }

        public async void AddToCart()
        {
            recipe.AddToCart();
            Db.Update(recipe);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = Constants.Texts.ToastAddToCart;
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

        public async void Delite()
        {
            var returnValue = await (new DeletePopUp()).ShowAsync();

            if (returnValue.status == DialogReturnStatuses.Positive)
            {
                Db.DeleteById(Id);
                await Shell.Current.GoToAsync("..");
            }
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
