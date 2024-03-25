using CookBoock.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Data
{
    internal abstract class DB<T>
    {
        public abstract void Add( T obj, Stream stream = null);
        public abstract void ClearAll();
        public abstract List<T> GetAll();
        public abstract T GetById(int id);
        protected LiteDatabase Db;
        public DB(string dbPath)
        {
            Db = new LiteDatabase("Filename=" + dbPath + ";Upgrate=true");
        }

        public void Close()
        {
            Db.Dispose();
        }


    }
}
