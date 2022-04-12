using KinoStudio_NET.Annotations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KinoStudio_NET.ViewModel
{
    internal class PassportDetails : INotifyPropertyChanged
    {
        private int _id;
        private int _fullNameId;
        private string _series;
        private string _number;
        private DateTime _dateOfIssue;
        private DateTime _dateOfBirth;
        private string _placeOfRegistration;
        private FullName _fullName;

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

        [JsonProperty(nameof(FullNameId))]
        public int FullNameId
        {
            get => _fullNameId;
            set
            {
                _fullNameId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Series))]
        public string Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Number))]
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(DateOfIssue))]
        public DateTime DateOfIssue
        {
            get => _dateOfIssue;
            set
            {
                _dateOfIssue = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(DateOfBirth))]
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(PlaceOfRegistration))]
        public string PlaceOfRegistration
        {
            get => _placeOfRegistration;
            set
            {
                _placeOfRegistration = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(FIO))]
        public FullName FIO
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public PassportDetails()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsNullOrDefault() => Series == default! || Number == default! || DateOfIssue == default ||
                                         DateOfBirth == default || PlaceOfRegistration == default! ||
                                         FIO == default! || FIO.IsNullOrDefault();

        public bool IsValid() => Series.Length == 4 && Number.Length == 6 && DateOfIssue <= DateTime.Now &&
                                 DateOfBirth < DateTime.Now.AddYears(-14) &&
                                 PlaceOfRegistration.Length is >= 10 and <= 200 && FIO.IsValid();
    }
}