using CookBoock.Data;
using CookBoock.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NativeMedia;
using Constants = CookBoock.Helpers.Constants;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace CookBoock.ViewModel
{
    public class AddPageViewModel : INotifyPropertyChanged
    {
        private RecipeDB Db;
        Recipe recipe;
        int Id { get; set; }
        public ICommand IngridientAdd { get; set; }
        public ICommand TagAdd { get; set; }
        public ICommand IngridientRemoveCommand { get; set; }
        public ICommand TagsRemoveCommand { get; set; }
        public ICommand saveData { get; set; }
        public ICommand ImageLoad { get; set; }
        Stream stream { get; set; }
        public string title;
        public string Title {
            get => title;
            set {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }
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
        public ObservableCollection<Tag> Tags
        {
            get => recipe.Tags;
            set
            {
                if (recipe.Tags != value)
                {
                    recipe.Tags = value;
                    OnPropertyChanged();
                }
            }
        }

        public AddPageViewModel()
        {
            Init();
            recipe = new Recipe();
            stream = null;
            Title = Constants.Texts.TitleAdd;
            saveData = new Command(Save);
        }

        public AddPageViewModel(string id)
        {
            Init();
            Id = Convert.ToInt32(id);
            recipe = Db.FindeById(Id);
            Image = ImageSource.FromStream(() => Db.GetStream(recipe.FileId));
            stream = null;
            Title = Constants.Texts.TitleRewrite;
            saveData = new Command(Update);
        }
        public void Init()
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            IngridientAdd = new Command(() =>
            {
                ingridients.Add(new Ingridients());
            });
            TagAdd = new Command(() =>
            {
                Tags.Add(new Tag());
            });
            IngridientRemoveCommand = new Command<Ingridients>(RemoveIngridient);
            TagsRemoveCommand = new Command<Tag>(RemoveTag);
            ImageLoad = new Command(SetImage);
        }

        private async void Save()
        {
            if (image != null && name.Trim(' ').Length != 0 && Tags.Count != 0 && ingridients.Count != 0 && cookingProcess.Trim().Length != 0)
            {
                recipe.SetFileId();
                stream = await files[0].OpenReadAsync();
                Db.Add(recipe, stream);
                stream.Close();
                Db.Close();
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                string text = Constants.Texts.ToastValidation;
                ToastDuration duration = ToastDuration.Short;
                double fontSize = 14;
                var toast = Toast.Make(text, duration, fontSize);
                await toast.Show(cancellationTokenSource.Token);
            }
        }

        private async void Update()
        {
            if (files == null)
            {
                stream = Db.GetStream(recipe.FileId);
            }
            else
            { 
                stream = await files[0].OpenReadAsync();
            }
            Db.FullUpdate(recipe, stream);
            stream.Close();
            Db.Close();
        }

        private void RemoveTag(Tag obj)
        {
            Tags.Remove(obj);
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
            Image = null;
#if ANDROID
            Image = ImageSource.FromStream(()=>stream);
#endif
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
