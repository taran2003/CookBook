using System.Collections.ObjectModel;

namespace CookBoock.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Ingridients> Ingridients { get; set; }
        public ObservableCollection<Tag> Tags { get; set; }
        public ObservableCollection<Step> Steps { get; set; }
        public string FileId { get; set; }
        public string SmallFileId { get; set; }
        public string ImageUrl { get; set; }
        public Microsoft.Maui.Graphics.IImage Image { get; set; }
        public bool IsCart { get; set; }
        public bool IsLoad { get; set; } = false;

        public Recipe() {
            Ingridients = new ObservableCollection<Ingridients>();
            Tags = new ObservableCollection<Tag>();
            Steps = new ObservableCollection<Step>();
            SetFileId();
        }

        public Recipe(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Tags = recipe.Tags;
            FileId = recipe.FileId;
            Steps = recipe.Steps;
            ImageUrl = recipe.ImageUrl;
            Ingridients = recipe.Ingridients;
        }

        public void SetFileId()
        {
            FileId = Guid.NewGuid().ToString();
            IsCart = false;
        }

        public void AddToCart()
        {
            IsCart = true;
        }

        public void RemoveFromCart()
        {
            IsCart = false;
        }
    }
}
