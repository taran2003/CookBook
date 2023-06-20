using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookBoock.Models
{
    public class Ingridients : INotifyPropertyChanged
    {
        public Ingridients() 
        { 

        }

        public Ingridients(string ingridient)
        {
            Ingridient = ingridient;
        }

        private string ingridient;
        public string Ingridient
        {
            get => ingridient;
            set
            {
                SetProperty(ref ingridient, value);
            }
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
