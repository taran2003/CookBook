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
            for (int i = 0; i < res.Count(); i++)
            {
                res[i].Image = GetImage(res[i].FileId);
            }
            return res;
        }

        public void Add(Recipe recipe, Stream stream)
        {
            Recipes.Insert(recipe);
            Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), stream);
        }

        public Recipe FindeById(int id)
        {
            return Recipes.FindOne(x => x.Id == id);
        }

        public ImageSource GetImage(string uuid)
        {
            var stream = FindImageById(uuid);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length - 1);
            stream.Dispose();
            return ImageSource.FromStream(() => new MemoryStream(data));
            //return null;  
        }

        public LiteFileStream<string> FindImageById(string id)
        {
            ImageStream = Fs.OpenRead(id);
            return ImageStream;
        }

        public void DeleteById(int id)
        {
            var recipe = FindeById(id);
            Fs.Delete(recipe.FileId);
            Recipes.Delete(id);
        } 
    }
}
