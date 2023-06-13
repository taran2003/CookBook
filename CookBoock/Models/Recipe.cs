using System.Collections.ObjectModel;

namespace CookBoock.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Ingridients> Ingridients { get; set; }
        public string CookingProcess { get; set; }
        public string FileId { get; set; }
        public ImageSource Image { get; set; }

        public Recipe() {
            Ingridients = new ObservableCollection<Ingridients>();
        }

        public void SetFileId()
        {
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
