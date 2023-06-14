using CookBoock.Data;
using CookBoock.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookBoock.ViewModel
{
    class RecipePageViewModel : INotifyPropertyChanged
    {
        private int Id;
        private RecipeDB Db;
        private Recipe recipe;
        public ICommand Delete {get; set;}
        public ICommand Rewrite { get; set;}
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

        public RecipePageViewModel(string id)
        {
            Id = Convert.ToInt32(id);
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            recipe = Db.FindeById(Id);
            name = recipe.Name;
            Ingridients = recipe.Ingridients;
            cookingProcess = recipe.CookingProcess;
            image = Db.GetImage(recipe.FileId);
            Delete = new Command(() =>
            {
                Db.DeleteById(Id);
            });

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
