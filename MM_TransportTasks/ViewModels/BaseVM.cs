using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MM_TransportTasks.ViewModels
{
    internal class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
