using KinoStudio_NET.Annotations;
using KinoStudio_NET.Commands;
using KinoStudio_NET.Globals;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.View;
using KinoStudio_NET.ViewModel.Abstracts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace KinoStudio_NET.ViewModel
{
    internal class OrganisationsViewModel : ViewModelsAbstract<Organisation>, INotifyPropertyChanged
    {
        private ObservableCollection<Organisation> _organistaions;
        private Organisation _selectedOrganisation;
        private bool _actorsPermission;
        private bool _employeesPermission;
        private bool _usersPermission;
        private bool _postersImagesPermission;

        public ObservableCollection<Organisation> Organisations
        {
            get => _organistaions;
            set
            {
                _organistaions = value;
                OnPropertyChanged();
            }
        }

        public Organisation SelectedOrganisation
        {
            get => _selectedOrganisation;
            set
            {
                _selectedOrganisation = value;
                OnPropertyChanged();
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

        public bool UsersPermissions
        {
            get => _usersPermission;
            set
            {
                _usersPermission = value;
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

        public OrganisationsViewModel()
        {
            (ActorsPermissions, EmployeesPermissions, _, UsersPermissions, PostersImagesPermissions) =
                UserGlobal.GetPermissions();
            Task.Run(async () => Organisations = await Organisations.GetAll());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string Path { get; set; } = "Organisations";

        private DelegateCommand _addOrganisation;

        public DelegateCommand AddOrganisation
        {
            get
            {
                return _addOrganisation ??= new(async _ =>
                {
                    if (Organisations.All(organisation => organisation.Id > 0))
                        Organisations.Add(new Organisation() {Status = true});
                });
            }
        }

        private DelegateCommand _updateOrganisation;

        public DelegateCommand UpdateOrganisations
        {
            get { return _updateOrganisation ??= new(async _ => { Organisations = await Organisations.GetAll(); }); }
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


        private DelegateCommand _toUsers;

        public DelegateCommand ToUsers
        {
            get
            {
                return _toUsers ??= new(async _ =>
                {
                    UsersView usersView = new();
                    usersView.Show();
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