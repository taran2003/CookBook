using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CookBoock.Data;
using CookBoock.Helpers;
using CookBoock.Models;
using NativeMedia;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Constants = CookBoock.Helpers.Constants;

namespace CookBoock.ViewModel
{
    public class AddPageViewModel : INotifyPropertyChanged
    {
        private RecipeDB Db;
        private StepDB StepDB;
        Recipe recipe;
        int Id { get; set; }
        public ICommand IngridientAdd { get; set; }
        public ICommand TagAdd { get; set; }
        public ICommand StepAdd { get; set; }
        public ICommand IngridientRemoveCommand { get; set; }
        public ICommand TagsRemoveCommand { get; set; }
        public ICommand StepsRemoveCommand { get; set; }
        public ICommand saveData { get; set; }
        public ICommand ImageLoad { get; set; }
        public ICommand StepImageLoad { get; set; }
        List<Stream> streams = new List<Stream>();
        public string title;
        public string Title
        {
            get => title;
            set
            {
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
            set
            {
                if (recipe.Name != value)
                {
                    recipe.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        ObservableCollection<Step> steps;
        public ObservableCollection<Step> Steps
        {
            get => steps;
            set
            {
                if (steps != value)
                {
                    steps = value;
                    OnPropertyChanged();
                }
            }
        }
        CancellationTokenSource cts = new CancellationTokenSource();
        List<IMediaFile> files = new List<IMediaFile>();
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
            Title = Constants.Texts.TitleAdd;
            saveData = new Command(Save);
        }

        public AddPageViewModel(string id)
        {
            Init();
            Id = Convert.ToInt32(id);
            recipe = Db.GetById(Id);
            Steps = recipe.Steps;
            streams[0] = Db.FindImageById(recipe.FileId);
            for (int i = 0; i < Steps.Count; i++)
            {
                streams.Add(Db.FindImageById(steps[i].FileId));
                files.Add(null);
            }
            for (int i = 0; i < Steps.Count; i++)
            {
                steps[i].Image = null;
                steps[i].Image = ImageSource.FromStream(() => streams[i + 1]);
            }
            Image = null;
            Image = ImageSource.FromStream(() => streams[0]);
            Title = Constants.Texts.TitleRewrite;
            saveData = new Command(Update);
        }
        public void Init()
        {
            Db = new RecipeDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            StepDB = new StepDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Recipes.db"));
            files.Add(null);
            steps = new ObservableCollection<Step>();
            streams.Add(null);
            IngridientAdd = new Command(() =>
            {
                ingridients.Add(new Ingridients());
            });
            TagAdd = new Command(() =>
            {
                Tags.Add(new Tag());
            });
            StepAdd = new Command(() =>
            {
                steps.Add(new Step());
                files.Add(null);
                streams.Add(null);

            });
            IngridientRemoveCommand = new Command<Ingridients>(RemoveIngridient);
            TagsRemoveCommand = new Command<Tag>(RemoveTag);
            StepsRemoveCommand = new Command<Step>(RemoveStep);
            ImageLoad = new Command(SetImage);
            StepImageLoad = new Command<Step>(SetStepImage);
        }

        private async void Save()
        {
            if (image != null && name.Trim(' ').Length != 0 && Tags.Count != 0 && ingridients.Count != 0 && Steps.Count != 0)
            {
                recipe.SetFileId();
                for (int i = 0; i < files.Count; i++)
                {
                    streams[i] = await files[i].OpenReadAsync();
                }
                recipe.Steps = Steps;
                Db.Add(recipe, streams);
                foreach (var stream in streams)
                {
                    stream.Close();
                }
                StepDB.Close();
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
            bool[] buf = new bool[files.Count];
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] != null)
                {
                    buf[i] = true;
                }
            }
            for (int i = 0; i < files.Count; i++)
            {
                if (buf[i])
                {
                    streams[i] = await files[i].OpenReadAsync();
                }
                else
                {
                    if (i == 0)
                    {
                        streams[i] = Db.FindImageById(recipe.FileId);
                    }
                    else
                    {
                        streams[i] = Db.FindImageById(recipe.Steps[i-1].FileId);
                    }
                }
            }
            Db.FullUpdate(recipe, streams);
            foreach (var stream in streams)
            {
                stream.Close();
            }
            Db.Close();
        }


        private async void SetImage()
        {
            try
            {
                var res = await MediaGallery.PickAsync(1, MediaFileType.Image);
                files[0] = res.Files.First();
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
            if (files == null || files.Count == 0)
            {
                cts.Dispose();
                return;
            }
            streams[0] = await files[0].OpenReadAsync();
            Image = null;
#if ANDROID
            Image = ImageSource.FromStream(()=>streams[0]);
#endif
        }

        private async void SetStepImage(Step obj)
        {
            var index = steps.IndexOf(obj) + 1;
            try
            {
                var res = await MediaGallery.PickAsync(1, MediaFileType.Image);
                files[index] = res.Files.First();
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
            if (files == null || files.Count == 0)
            {
                cts.Dispose();
                return;
            }
            streams[index] = await files[index].OpenReadAsync();
            steps[index - 1].Image = null;
#if ANDROID
            steps[index - 1].Image = ImageSource.FromStream(() => streams[index]);
#endif
        }

        private void RemoveIngridient(Ingridients obj)
        {
            ingridients.Remove(obj);
        }

        private void RemoveTag(Tag obj)
        {
            Tags.Remove(obj);
        }

        private void RemoveStep(Step obj)
        {
            var index = steps.IndexOf(obj)+1;
            files.RemoveAt(index);
            streams.RemoveAt(index);
            Steps.Remove(obj);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
