﻿using System.Collections.ObjectModel;

namespace CookBoock.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Ingridients> Ingridients { get; set; }
        public string CookingProcess { get; set; }
        public string FileId { get; set; }
        public string ImageUrl { get; set; }
        public ImageSource Image { get; set; }
        public bool IsCart { get; set; }

        public Recipe() {
            Ingridients = new ObservableCollection<Ingridients>();
            SetFileId();
        }

        public Recipe(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Ingridients = recipe.Ingridients;
            CookingProcess = recipe.CookingProcess;
            FileId = recipe.FileId;
            ImageUrl = recipe.ImageUrl;
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
