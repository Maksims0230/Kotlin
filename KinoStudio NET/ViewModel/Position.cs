using KinoStudio_NET.Annotations;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KinoStudio_NET.ViewModel
{
    internal class Position : ViewModelAbstract, INotifyPropertyChanged
    {
        private int _id = 0;
        private string _position1;

        [JsonProperty("ID")]
        public override int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("Position1")]
        public string Position1
        {
            get => _position1;
            set
            {
                _position1 = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public override string Path { get; set; } = "Positions";

        public Position()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsNullOrDefault() => this == default! && this.Id == default || this.Position1 == default!;
    }
}