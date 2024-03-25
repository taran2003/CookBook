using CookBoock.Helpers;
using CookBoock.Models;
using LiteDB;
using System.Collections.ObjectModel;

namespace CookBoock.Data
{
    class RecipeDB : DB<Recipe>
    {
        private ILiteStorage<string> Fs;
        private ILiteCollection<Recipe> Recipes;
        private LiteFileStream<string> ImageStream;
        public RecipeDB(string dbPath) : base( dbPath)
        {             
            Fs = Db.FileStorage;
            Recipes = Db.GetCollection<Recipe>();
        }

        public override List<Recipe> GetAll()
        {
            List<Recipe> res  = Recipes.FindAll().ToList();
            Parallel.For(0, res.Count(), (int i) =>
            {
                res[i].Image = GetImage(res[i].FileId, true);
            });
            ImageGeter.ClearImageCashe();
            return res;
        }

        public override void Add(Recipe recipe, Stream stream)
        {
            Recipes.Insert(recipe);
            Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), stream);
        }

        public void Add(Recipe recipe, List<Stream> streams)
        {
            for (int i = 1; i < streams.Count; i++)
            {
                //SDB.Add(steps[i]);
                //recipe.StepsId.Add(steps[i].Id);
                recipe.Steps[i-1].SetFileId();
                recipe.Steps[i - 1].Image = null;
                Fs.Upload(recipe.Steps[i-1].FileId, Guid.NewGuid().ToString(), streams[i]);
            }
            Recipes.Insert(recipe);
            Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), streams[0]);
        }

        public void Add(Recipe recipe)
        {
            if (GetById(recipe.Id) == null)
            {
                Recipes.Insert(recipe);
                if (recipe.ImageUrl != null)
                {
                    Stream stream = ImageGeter.GetImageStreamFromUrl(recipe.ImageUrl);
                    Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), stream);
                }
            }
        }

        public void FullUpdate(Recipe recipe, List<Stream> streams)
        {
            for (int i = 1; i < streams.Count; i++)
            {
                if (recipe.Steps[i-1].FileId.Trim(' ').Count() == 0)
                {
                    recipe.Steps[i-1].SetFileId();
                }
                Fs.Upload(recipe.Steps[i-1].FileId, Guid.NewGuid().ToString(), streams[i]);
                recipe.Steps[i-1].Image = null;
            }
            Recipes.Update(recipe);
            Fs.Upload(recipe.FileId, Guid.NewGuid().ToString(), streams[0]);
        }

        public void Update(Recipe recipe)
        {
            for(int i = 0; i < recipe.Steps.Count; i++)
            {
                recipe.Steps[i].Image = null;
            }
            Recipes.Update(recipe);
        }

        public override Recipe GetById(int id )
        {
            return Recipes.FindOne(x => x.Id == id);
        }

        public Recipe GetById(int id, ref ObservableCollection<Step> steps)
        {

            var buf = Recipes.FindOne(x => x.Id == id);
            return buf;
        }

        public List<Recipe> FindeAllByCart()
        {
            List<Recipe> res = Recipes.Find(x => x.IsCart == true).ToList();
            Parallel.For(0, res.Count(), (int i) =>
            {
                res[i].Image = GetImage(res[i].FileId,true);
            });
            ImageGeter.ClearImageCashe();
            return res;
        }

        public Microsoft.Maui.Graphics.IImage GetImage(string uuid, bool IsSmall = false)
        {
            var stream = FindImageById(uuid);
            Microsoft.Maui.Graphics.IImage image = null;
            if (IsSmall)
            {
                image = ImageGeter.GetSmallImage(stream);
            }else
            {
                image = ImageGeter.GetImage(stream);
            }
            return image;
            //return null;  
        }

        public List<MemoryStream> GetStreams(Recipe recipe)
        {
            var stream = new List<LiteFileStream<string>>();
            stream.Add(FindImageById(recipe.FileId));
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                stream.Add(FindImageById(recipe.Steps[i].FileId));
            }
            var res = new List<MemoryStream>();
            for (int i = 0; i < stream.Count; i++)
            {
                byte[] data = new byte[stream[i].Length];
                stream[i].Read(data, 0, (int)stream[i].Length - 1);
                stream[i].Dispose();
                res.Add(new MemoryStream(data));
            }
            return res;
            //return null;  
        }

        public LiteFileStream<string> FindImageById(string id)
        {
            ImageStream = Fs.OpenRead(id);
            return ImageStream;
        }

        public void DeleteById(int id)
        {
            var recipe = GetById(id);
            Fs.Delete(recipe.FileId);
            foreach(var step in recipe.Steps)
            {
                Fs.Delete(step.FileId);
            }
            Recipes.Delete(id);
        }

        public override void ClearAll()
        {
            Recipes.DeleteAll();
        }
    }
}
