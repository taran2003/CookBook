using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Models
{
    class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Ingridients> Ingridients { get; set; }
        public string CookingProcess { get; set; }

        public Recipe() {
            Ingridients = new ObservableCollection<Ingridients>();
        }

        public Recipe(string name, ObservableCollection<Ingridients> ingridients, string cookingProcess)
        {
            Name = name;
            Ingridients = ingridients;
            CookingProcess = cookingProcess;
        }
    }
}
