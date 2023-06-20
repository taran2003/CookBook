using CookBoock.Models;
using LiteDB;

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
            recipe.ImageUrl = null;
            Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), stream);
        }

        public void Add(Recipe recipe)
        {
            Recipes.Insert(recipe);
            recipe.FileId = null;
            
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
            Thread threadOne;
            Thread threadTwo;
            threadOne = new Thread(() => {
                for (int i = 0; i < res.Count() / 2; i++)
                {
                    res[i].Image = GetImage(res[i].FileId);
                }
            });
            threadTwo = new Thread(() =>
            {
                for (int i = res.Count() / 2; i < res.Count(); i++)
                {
                    res[i].Image = GetImage(res[i].FileId);
                }
            });
            threadOne.Start();
            threadTwo.Start();
            threadOne.Join();
            threadTwo.Join();
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
