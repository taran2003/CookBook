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
        public LiteDatabase Db;
        public ILiteCollection<Recipe> Recipes;

        public RecipeDB(string dbPath)
        { 
            Db = new LiteDatabase("Filename="+dbPath+";Upgrate=true");
            Recipes = Db.GetCollection<Recipe>();
        }
    }
}
