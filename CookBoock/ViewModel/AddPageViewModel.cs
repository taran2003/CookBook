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
using static Java.Util.Jar.Attributes;

namespace CookBoock
{
    internal class AddPageViewModel : INotifyPropertyChanged
    {
        private RecipeDB Db;
        Recipe recipe;
        public ICommand ingridientAdd { get; set; }
        public ICommand ingridientRemove { get; set; }
        public ICommand saveData { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public string name
        {
            get => recipe.Name;
            set {
                if (recipe.Name != value)
                {
                    recipe.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string cookingProcess
        {
            get => recipe.CookingProcess;
            set
            {
                if (recipe.CookingProcess != value)
                {
                    recipe.CookingProcess = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Ingridients> ingridients
        {
            get => recipe.Ingridients;
            set
            {
                if (recipe.Ingridients != value)
                {
                    recipe.Ingridients = value;
                    OnPropertyChanged();
                }
            }
        }

        public AddPageViewModel()
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            recipe = new Recipe();
            ingridientAdd = new Command(()=>
            {
                ingridients.Add(new Ingridients());
            });
            ingridientRemove = new Command(() =>
            {
                if (ingridients.Count > 0)
                {
                    ingridients.RemoveAt(ingridients.Count - 1);
                }
            });
            saveData = new Command(() =>
            {
                Db.Add(recipe);
                Db.Close();
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
