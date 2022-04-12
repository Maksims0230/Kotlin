using KinoStudio_NET.Annotations;
using KinoStudio_NET.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KinoStudio_NET.ViewModel
{
    internal class User : INotifyPropertyChanged
    {
        private int _id;
        private string? _login;
        private string? _password;
        private int _roleId;
        private ObservableCollection<Role> _roles = new();

        public User(int id, string login, string password, int roleId)
        {
            _id = id;
            _login = login;
            _password = password;
            _roleId = roleId;
            Roles = new(UserModel.GetRoles());
        }

        public User()
        { 
            Roles = new(UserModel.GetRoles());
        }

        public void Deconstruct([NotNull] out string login, [NotNull] out string password, out int roleId)
        {
            login = _login!;
            password = _password!;
            roleId = _roleId;
        }

        [JsonProperty("UserId")]
        public int Id
        {
            get => _id;
            set
            {
                if (_id == default)
                    _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("Login")]
        public string Login
        {
            get => _login!;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("Password")]
        public string Password
        {
            get => _password!;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("RoleId")]
        public int RoleId
        {
            get => _roleId;
            set
            {
                _roleId = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public Role? SelectedRole
        {
            get => Roles.FirstOrDefault(role => role.Id == RoleId);
            set
            {
                if (RoleId != value!.Id)
                    RoleId = value.Id;
            }
        }

        [JsonIgnore]
        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged();
            }
        }

        public void SetValues(int id, string? login, string? password, int roleId)
        {
            _id = id;
            _login = login;
            _password = password;
            _roleId = roleId;
        }

        public override string ToString()
        {
            return $"{Id},{Login},{Password},{RoleId}";
        }

        [JsonIgnore] public bool Status { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual async void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!Status && !this.IsNullOrDefault())
            {
                Status = true;
                return;
            }

            if (!Status || this.IsNullOrDefault()) return;

            if (propertyName != nameof(Id) && !this.IsNullOrDefault())
            {
                if (await this.ExistsById())
                {
                    await this.Update();
                }
                else
                {
                    this.Id = (await this.Add()).Id;
                }
            }
        }
    }
}