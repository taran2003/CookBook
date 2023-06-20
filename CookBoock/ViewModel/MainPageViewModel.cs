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
    class MainPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Recipe> recipesList;
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
            RecipesList = new ObservableCollection<Recipe>(RecipeApi.GetRecipes(30));
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
