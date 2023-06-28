using CookBoock.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace CookBoock.ViewModel
{
    class DeletePopUpViewModel : INotifyPropertyChanged
    {
        public Command Confirm { get; set; }
        public Command Reject { get; set; }

        public DeletePopUpViewModel()
        {
        }

        private readonly TaskCompletionSource<DialogReturnValue> _taskCompletionSource = new();

        public Task<DialogReturnValue> ReturnValueAsync()
        {
            return _taskCompletionSource.Task;
        }

        public void SetReturnValue(DialogReturnStatuses status)
        {
            _taskCompletionSource.TrySetResult(new DialogReturnValue(status));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
