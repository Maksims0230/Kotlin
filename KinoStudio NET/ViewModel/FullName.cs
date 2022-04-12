using KinoStudio_NET.Annotations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace KinoStudio_NET.ViewModel
{
    internal class FullName : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _surname;
        private string _patronymic;

        [JsonProperty("ID")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Name))]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Surname))]
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Patronymic))]
        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged();
            }
        }

        public FullName()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsNullOrDefault() => Name == default! && Surname == default! && Patronymic == default!;

        public bool IsValid() => Regex.IsMatch(Name, "(^[A-ZА-Я][a-zа-я]+)$") &&
                                 Regex.IsMatch(Surname, "(^[A-ZА-Я][a-zа-я]+)$") &&
                                 Regex.IsMatch(Surname, "(^([A-ZА-Я][a-zа-я]+|[ ]?))$");
    }
}