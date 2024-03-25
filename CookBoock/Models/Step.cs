using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Models
{
    public class Step: INotifyPropertyChanged
    {
        private string cookingProcess;
        public string CookingProcess
        {
            get
            {
                return cookingProcess;
            }
            set
            {
                SetProperty(ref cookingProcess, value);
            }
        }
        private string fileId;
        public string FileId { get { return fileId; } set { fileId = value; } }
        public string ImageUrl { get; set; }
        private ImageSource image;
        public ImageSource Image
        {
            get => image;
            set
            {
                SetProperty(ref image, value);
            }
        }
        public Step()
        {
            fileId = "";
            CookingProcess = "";
            Image = null;
        }

        public Step(Step step)
        {
            CookingProcess = step.CookingProcess;
            fileId = step.FileId;
            ImageUrl = step.ImageUrl;
            Image = step.Image;
        }

        public void SetFileId()
        {
            fileId = Guid.NewGuid().ToString();
        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
