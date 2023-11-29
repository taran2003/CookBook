using CookBoock.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Data
{
    internal class IngridientDB : DB
    {

        private ILiteCollection<Ingridients> Ingridients;

        public IngridientDB(string filePath) : base(filePath)
        {

        }

        public void AddTag(Ingridients ingridient)
        {
            var buf = Ingridients.FindOne(x => x.Ingridient.Trim(' ') == ingridient.Ingridient.Trim(' '));
            if (buf == null)
            {
                Ingridients.Insert(ingridient);
            }
        }

        public List<Ingridients> GetAll()
        {
            List<Ingridients> res = Ingridients.FindAll().ToList();
            return res;
        }

        public void ClearAll()
        {
            Ingridients.DeleteAll();
        }
    }
}
