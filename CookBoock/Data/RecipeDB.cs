using CookBoock.Models;
using Java.Net;
using LiteDB;
using System.Net;
using System.Text.RegularExpressions;

namespace CookBoock.Data
{
    class RecipeDB
    {
        private LiteDatabase Db;
        private ILiteStorage<string> Fs;
        private ILiteCollection<Recipe> Recipes;
        private LiteFileStream<string> ImageStream;

        public RecipeDB(string dbPath)
        { 
            Db = new LiteDatabase("Filename="+dbPath+";Upgrate=true");
            Fs = Db.FileStorage;
            Recipes = Db.GetCollection<Recipe>();
        }

        public void Close()
        {
            Db.Dispose();
        }

        public List<Recipe> GetAll()
        {
            List<Recipe> res  = Recipes.FindAll().ToList();
            Parallel.For(0, res.Count(), (int i) =>
            {
                res[i].Image = GetImage(res[i].FileId);
            });
            ClearCashe();
            return res;
        }

        public void Add(Recipe recipe, Stream stream)
        {
            Recipes.Insert(recipe);
            Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), stream);
        }

        public void Add(Recipe recipe)
        {
            if (FindeById(recipe.Id) == null)
            {
                Recipes.Insert(recipe);
                if (recipe.ImageUrl != null)
                {
                    WebClient client = new WebClient();
                    byte[] imageBytes = new byte[0];
                    client.OpenReadCompleted += (s, e) =>
                    {
                        imageBytes = new byte[e.Result.Length];
                        e.Result.Read(imageBytes, 0, imageBytes.Length);
                    };
                    Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), client.OpenRead(new Uri(recipe.ImageUrl)));
                }
            }
        }

        public void FullUpdate(Recipe recipe, Stream stream)
        {
            Recipes.Update(recipe);
            Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), stream);
        }

        public void Update(Recipe recipe)
        {
            Recipes.Update(recipe);
        }

        public Recipe FindeById(int id)
        {
            return Recipes.FindOne(x => x.Id == id);
        }

        public List<Recipe> FindeAllByCart()
        {
            List<Recipe> res = Recipes.Find(x => x.IsCart == true).ToList();
            Parallel.For(0, res.Count(), (int i) =>
            {
                res[i].Image = GetImage(res[i].FileId);
            });
            ClearCashe();
            return res;
        }

        public List<Recipe> FindeAllByName(string name)
        {
            Regex regex = new Regex($"\\w*{name}\\w*");
            List<Recipe> res = Recipes.FindAll()?.Where(x => regex.IsMatch(x.Name)).ToList();
            Parallel.For(0, res.Count(), (int i) =>
            {
                res[i].Image = GetImage(res[i].FileId);
            });
            ClearCashe();
            return res;
        }

        public ImageSource GetImage(string uuid)
        {
            var stream = FindImageById(uuid);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length - 1);
            stream.Close();
            return ImageSource.FromStream(() => new MemoryStream(data));
            //return null;  
        }

        public MemoryStream GetStream(string uuid)
        {
            var stream = FindImageById(uuid);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length - 1);
            stream.Dispose();
            return new MemoryStream(data);
            //return null;  
        }

        public LiteFileStream<string> FindImageById(string id)
        {
            ImageStream = Fs.OpenRead(id);
            return ImageStream;
        }

        private void ClearCashe()
        {
            var imageManagerDiskCache = Path.Combine(FileSystem.CacheDirectory, "image_manager_disk_cache");

            if (Directory.Exists(imageManagerDiskCache))
            {
                foreach (var imageCacheFile in Directory.EnumerateFiles(imageManagerDiskCache))
                {
                    File.Delete(imageCacheFile);
                }
            }
        }

        public void DeleteById(int id)
        {
            var recipe = FindeById(id);
            Fs.Delete(recipe.FileId);
            Recipes.Delete(id);
        } 
    }
}
