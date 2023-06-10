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

        public IEnumerable<Recipe> GetAll()
        {
            return Recipes.FindAll();
        }

        public void Add(Recipe recipe, string filePath)
        {
            Recipes.Insert(recipe);
            Fs.Upload(recipe.FileId, filePath);
        }

        public Recipe FindeById(int id)
        {
            return Recipes.FindOne(x => x.Id == id);
        }

        public LiteFileStream<string> ImageFindById(string id)
        {
            ImageStream = Fs.OpenRead(id);
            return ImageStream;
        }

        public void DeleteById(int id)
        {
            Recipes.Delete(id);
        } 
    }
}
