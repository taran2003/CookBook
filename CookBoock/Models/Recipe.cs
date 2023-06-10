using System.Collections.ObjectModel;

namespace CookBoock.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Ingridients> Ingridients { get; set; }
        public string CookingProcess { get; set; }

        public string FileId;

        public Recipe() {
            Ingridients = new ObservableCollection<Ingridients>();
            FileId = Guid.NewGuid().ToString();
        }

        public Recipe(string name, ObservableCollection<Ingridients> ingridients, string cookingProcess)
        {
            Name = name;
            Ingridients = ingridients;
            CookingProcess = cookingProcess;
            FileId = Guid.NewGuid().ToString();
        }

        public Recipe(string name, ObservableCollection<Ingridients> ingridients, string cookingProcess, string fileId)
        {
            Name = name;
            Ingridients = ingridients;
            CookingProcess = cookingProcess;
            FileId = fileId;
        }
    }
}
