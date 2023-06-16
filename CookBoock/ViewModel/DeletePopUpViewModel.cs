using CookBoock.View;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookBoock.ViewModel
{
    class DeletePopUpViewModel : INotifyPropertyChanged
    {
        public Command Confirm { get; set; }
        public Command Reject { get; set; }

        public DeletePopUpViewModel()
        {
            Confirm = new Command(async () =>
            {
                await MopupService.Instance.PopAsync();
                MessagingCenter.Send<DeletePopUpViewModel,bool>(this, "confirm", true);
            });
            Reject = new Command(async () =>
            {
                await MopupService.Instance.PopAsync();
                MessagingCenter.Send<DeletePopUpViewModel, bool>(this, "confirm", false);
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
