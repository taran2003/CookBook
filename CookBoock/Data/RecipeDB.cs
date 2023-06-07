using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBoock.Models;
using LiteDB;

namespace CookBoock.Data
{
    class RecipeDB
    {
        private LiteDatabase Db;
        private ILiteCollection<Recipe> Recipes;

        public RecipeDB(string dbPath)
        { 
            Db = new LiteDatabase("Filename="+dbPath+";Upgrate=true");
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

        public void Add(Recipe recipe)
        {
            Recipes.Insert(recipe);
        }

        public Recipe FindeById(int id)
        {
            return Recipes.FindOne(x => x.Id == id);
        }

        public void DeleteById(int id)
        {
            Recipes.Delete(id);
        } 
    }
}
