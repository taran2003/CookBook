using CookBoock.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Data
{
    internal class TagDB : DB
    {
        private ILiteCollection<Tag> Tags;

        public TagDB(string filePath) : base (filePath) 
        {
            
        }

        public void AddTag(Tag tag)
        {
            var buf = Tags.FindOne(x => x._Tag.Trim(' ') == tag._Tag.Trim(' '));
            if (buf == null)
            {
                Tags.Insert(tag);
            }
        }

        public List<Tag> GetAll()
        {
            List<Tag> res = Tags.FindAll().ToList(); 
            return res;
        }

        public void ClearAll()
        {
            Tags.DeleteAll();
        }
    }
}
