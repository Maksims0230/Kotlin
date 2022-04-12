using KinoStudio_NET.Annotations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KinoStudio_NET.ViewModel
{
    internal class Role : INotifyPropertyChanged
    {
        private int _id;
        private string? _name;

        public Role(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public Role()
        {
        }

        [JsonProperty("Id")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("Role1")]
        public string Name
        {
            get => _name!;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}