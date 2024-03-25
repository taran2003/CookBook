using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookBoock.Models
{
    public class Ingridients : INotifyPropertyChanged
    {
        public Ingridients() 
        { 
            _Ingridient = "";
        }

        public Ingridients(string ingridient)
        {
            _Ingridient = ingridient;
        }

        private string ingridient;
        public string _Ingridient
        {
            get => ingridient;
            set
            {
                SetProperty(ref ingridient, value);
            }
        }

        public override bool Equals(object obj)
        {
            var item = obj as Ingridients;
            if (item == null) return false;
            return this._Ingridient.Equals(item._Ingridient);
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
