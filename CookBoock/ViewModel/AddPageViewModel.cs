using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CookBoock.ViewModel
{
    public class AddPageViewModel : INotifyPropertyChanged
    {
        private RecipeDB Db;
        Recipe recipe;
        public ICommand ingridientAdd { get; set; }
        public ICommand IngridientRemoveCommand { get; set; }
        public ICommand saveData { get; set; }
        public ICommand ImageLoad { get; set; }

        private string image;
        public string Image 
        { 
            get => image;
            set 
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged();
                }
            } 
        }
        
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

            IngridientRemoveCommand = new Command<Ingridients>(RemoveIngridient);

            saveData = new Command(() =>
            {
                Db.Add(recipe,Image);
                Db.Close();
            });
            ImageLoad = new Command(async () => {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images
                    
                });
                if (result != null)
                {
                    Image = result.FullPath;
                }
            });
        }

        private void RemoveIngridient(Ingridients obj)
        {
            ingridients.Remove(obj);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
