using KinoStudio_NET.Annotations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace KinoStudio_NET.ViewModel
{
    internal class ContactDetails : INotifyPropertyChanged
    {
        private int _id;
        private string _workPhone;
        private string _homePhone;
        private string _email;

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

        [JsonProperty(nameof(WorkPhone))]
        public string WorkPhone
        {
            get => _workPhone;
            set
            {
                _workPhone = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(HomePhone))]
        public string HomePhone
        {
            get => _homePhone;
            set
            {
                _homePhone = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Email))]
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public ContactDetails()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsEmptyOrDefault() => WorkPhone == default! || HomePhone == default! || Email == default!;

        public bool IsValid() =>
            Regex.IsMatch(WorkPhone, @"^(([ ]?|([+]?[7]|[8])[ ]?[(]?\d{3}[)]?[ ]?\d{3}[ -]?\d{2}[ -]?\d{2}))$")
            && Regex.IsMatch(HomePhone, @"^(([ ]?|([+]?[7]|[8])[ ]?[(]?\d{3}[)]?[ ]?\d{3}[ -]?\d{2}[ -]?\d{2}))$")
            && Regex.IsMatch(Email,
                @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$");
    }
}