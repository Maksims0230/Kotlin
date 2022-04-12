using KinoStudio_NET.Annotations;
using KinoStudio_NET.Commands;
using KinoStudio_NET.Globals;
using KinoStudio_NET.Models;
using KinoStudio_NET.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KinoStudio_NET.ViewModel
{
    internal class UsersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users;
        private User? _selectedUser;
        private bool _addUserButtonIsEnabled = true;
        private string _searchLine;
        private bool _actorsPermission;
        private bool _employeesPermission;
        private bool _organisationsPermission;
        private bool _postersImagesPermission;

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser!;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public bool AddUserButtonIsEnabled
        {
            get => _addUserButtonIsEnabled;
            set
            {
                _addUserButtonIsEnabled = value;
                OnPropertyChanged();
            }
        }

        public string SearchLine
        {
            get => _searchLine;
            set
            {
                _searchLine = value;
                this.SelectedUser = string.IsNullOrWhiteSpace(_searchLine)
                    ? null!
                    : this.Users.FirstOrDefault(user => user.Login.Contains(value))!;
                OnPropertyChanged();
            }
        }

        private DelegateCommand? _addUser;

        public DelegateCommand AddUser
        {
            get
            {
                return _addUser ??= new(_ =>
                {
                    if (this.ExistsIsNullOrEmptyUser())
                    {
                        AddUserButtonIsEnabled = false;
                    }
                    else
                    {
                        this.Users.Add(new() { Status = true });
                        AddUserButtonIsEnabled = true;
                    }
                });
            }
        }

        private DelegateCommand _toCSV;

        public DelegateCommand ToCSV
        {
            get
            {
                return _toCSV ??= new(_ => { this.ExportCSV(); }
                );
            }
        }

        private DelegateCommand _ofCSV;

        public DelegateCommand OfCSV
        {
            get
            {
                return _ofCSV ??= new(async _ => { Users = await this.ImportCSV(); }
                );
            }
        }

        private DelegateCommand? _updateUsers;

        public DelegateCommand UpdateUsers
        {
            get { return _updateUsers ??= new(async _ => Users = new(await UsersModel.GetUsersAsync())); }
        }

        private DelegateCommand? _deleteUser;

        public DelegateCommand DeleteUser
        {
            get
            {
                return _deleteUser ??= new(async _ =>
                {
                    if (this._selectedUser != null && await this._selectedUser.Delete())
                        Users.Remove(_selectedUser!);
                    else MessageBox.Show("Ошибка при удалении.");
                });
            }
        }

        public bool ActorsPermissions
        {
            get => _actorsPermission;
            set
            {
                _actorsPermission = value;
                OnPropertyChanged();
            }
        }

        public bool EmployeesPermissions
        {
            get => _employeesPermission;
            set
            {
                _employeesPermission = value;
                OnPropertyChanged();
            }
        }

        public bool OrganisationsPermissions
        {
            get => _organisationsPermission;
            set
            {
                _organisationsPermission = value;
                OnPropertyChanged();
            }
        }

        public bool PostersImagesPermissions
        {
            get => _postersImagesPermission;
            set
            {
                _postersImagesPermission = value;
                OnPropertyChanged();
            }
        }

        public UsersViewModel()
        {
            (ActorsPermissions, EmployeesPermissions, OrganisationsPermissions, _, PostersImagesPermissions) =
                UserGlobal.GetPermissions();
            _users = new(UsersModel.GetUsers());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DelegateCommand _toActors;

        public DelegateCommand ToActors
        {
            get
            {
                return _toActors ??= new(_ =>
                {
                    ActorsView actorsView = new();
                    actorsView.Show();
                    this.Close?.Invoke();
                });
            }
        }

        private DelegateCommand _toEmployees;

        public DelegateCommand ToEmployees
        {
            get
            {
                return _toEmployees ??= new(async _ =>
                {
                    EmployeesView employeesView = new();
                    employeesView.Show();
                    this.Close?.Invoke();
                });
            }
        }

        private DelegateCommand _toOrganisations;

        public DelegateCommand ToOrganisations
        {
            get
            {
                return _toOrganisations ??= new(async _ =>
                {
                    OrganisationsView organisations = new();
                    organisations.Show();
                    this.Close?.Invoke();
                });
            }
        }

        private DelegateCommand _toPostersImages;

        public DelegateCommand ToPostersImages
        {
            get
            {
                return _toPostersImages ??= new(async _ =>
                {
                    PostersImagesView imagesPostersView = new();
                    imagesPostersView.Show();
                    this.Close?.Invoke();
                });
            }
        }

        public Action Close;
    }
}