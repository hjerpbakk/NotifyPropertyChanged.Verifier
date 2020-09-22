using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NotifyPropertyChanged.Verifier.Tests.ViewModels
{
    public sealed class ViewModel : INotifyPropertyChanged
    {
        int backingField;
        string backingField2;

        public int PropertyWithNotify
        {
            get => backingField;
            set => Set(ref backingField, value);
        }

        public string PropertyWithMultipleNotifies
        {
            get => backingField2;
            set
            {
                PropertyWithNotify = int.Parse(value);
                Set(ref backingField2, value);
            }
        }

        public string PropertyWithSuperfluousNotifies
        {
            get => backingField2;
            set
            {
                PropertyWithMultipleNotifies = value;
                OnPropertyChanged("UnusedNotification");
            }
        }

        public int PropertyWithoutNotify { get; set; }

        public async Task UpdatePropertyUsingTask(int newValue)
            => await Task.Run(() => PropertyWithNotify = newValue);

        public async ValueTask UpdatePropertyUsingValueTask(int newValue)
            => await Task.Run(() => PropertyWithNotify = newValue);

        public event PropertyChangedEventHandler PropertyChanged;

        private bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
