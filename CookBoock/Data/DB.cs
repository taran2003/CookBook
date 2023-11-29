using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Data
{
    internal abstract class DB
    {
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
