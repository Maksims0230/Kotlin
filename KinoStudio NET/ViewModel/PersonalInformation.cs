using KinoStudio_NET.Annotations;
using KinoStudio_NET.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KinoStudio_NET.ViewModel
{
    internal class PersonalInformation : INotifyPropertyChanged
    {
        private int _id;
        private int _userId;
        private int _passportDetailsId;
        private int _contactDetailsId;
        private ContactDetails _contact;
        private PassportDetails _passport;
        private ObservableCollection<User> _users;

        [JsonProperty("personalInformationId")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("userId")]
        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId == value) return;
                _userId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("passportDetailsId")]
        public int PassportDetailsId
        {
            get => _passportDetailsId;
            set
            {
                _passportDetailsId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("contactDetailsId")]
        public int ContactDetailsId
        {
            get => _contactDetailsId;
            set
            {
                _contactDetailsId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("contact")]
        public ContactDetails Contact
        {
            get => _contact;
            set
            {
                _contact = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("passport")]
        public PassportDetails Passport
        {
            get => _passport;
            set
            {
                _passport = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public User? SelectedUser
        {
            get => Users?.FirstOrDefault(usr => usr.Id == UserId) ?? null;
            set
            {
                if (UserId != value!.Id)
                    UserId = value.Id;
            }
        }

        public PersonalInformation()
        {
            Users = new ObservableCollection<User>(UsersModel.GetUsers());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsNullOrDefault() => UserId == default || Contact == default! || Passport == default! ||
                                         Contact.IsEmptyOrDefault() || Passport.IsNullOrDefault();

        public bool IsValid() => Contact.IsValid() && Passport.IsValid();
    }
}