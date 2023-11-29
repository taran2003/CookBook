using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.Models
{
    public class Tag : INotifyPropertyChanged
    {
        public Tag()
        {

        }

        public Tag(string tag)
        {
            _Tag = tag;
        }

        private string _tag;
        public string _Tag
        {
            get => _tag;
            set
            {
                SetProperty(ref _tag, value);
            }
        }

        public override bool Equals(object obj)
        {
            var item = obj as Tag;
            if (item == null) return false;
            return this._Tag.Equals(item._Tag);
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
