using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NativeMedia;

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
        private ImageSource image;
        public ImageSource Image 
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
        CancellationTokenSource cts = new CancellationTokenSource();
        IMediaFile[] files = null;
        Stream stream { get; set; }

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
            stream = null;
            ingridientAdd = new Command(()=>
            {
                ingridients.Add(new Ingridients());
            });
            IngridientRemoveCommand = new Command<Ingridients>(RemoveIngridient);
            saveData = new Command(Save);
            ImageLoad = new Command(SetImage);
        }

        private async void Save()
        {
            recipe.SetFileId();
            stream = await files[0].OpenReadAsync();
            Db.Add(recipe, stream);
            Db.Close();
        }

        private async void SetImage()
        {
            try
            {
                var res = await MediaGallery.PickAsync(1, MediaFileType.Image);
                files = res?.Files?.ToArray();
            }
            catch (OperationCanceledException)
            {
                files = null;
            }
            catch (Exception)
            {
                files = null;
            }
            finally
            {
                cts.Dispose();
            }
            if (files == null || files.Length == 0)
            {
                cts.Dispose();
                return; 
            }
            stream = await files[0].OpenReadAsync();
            ImageSource c =
            Image = null;
            Image = ImageSource.FromStream(() => stream);
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
