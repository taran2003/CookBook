using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Models
{
    class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Ingridients { get; set; }
        public string CookingProcess { get; set; }

        public Recipe() { }

        public Recipe(string name, string[] ingridients, string cookingProcess)
        {
            Name = name;
            Ingridients = ingridients;
            CookingProcess = cookingProcess;
        }
    }
}
