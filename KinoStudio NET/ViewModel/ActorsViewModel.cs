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
using System.Windows;

namespace KinoStudio_NET.ViewModel
{
    internal class ActorsViewModel : ViewModelsAbstract<Actor>, INotifyPropertyChanged
    {
        private ObservableCollection<Actor> _actors = new();
        private Actor _selectedActor;
        private string _searchLine = "";
        private bool _employeesPermission;
        private bool _organisationsPermission;
        private bool _usersPermission;
        private bool _postersImagesPermission;

        public ObservableCollection<Actor> Actors
        {
            get => _actors;
            set
            {
                _actors = value;
                OnPropertyChanged();
            }
        }

        public Actor SelectedActor
        {
            get => _selectedActor;
            set
            {
                if (Equals(value, _selectedActor)) return;
                _selectedActor = value;
                OnPropertyChanged();
            }
        }

        public string SearchLine
        {
            get => _searchLine;
            set
            {
                _searchLine = value;
                this.SelectedActor = string.IsNullOrWhiteSpace(_searchLine)
                    ? null!
                    : this.Actors.FirstOrDefault(actor =>
                        $"{actor.PersonalInfo.Passport.FIO.Surname} {actor.PersonalInfo.Passport.FIO.Name} {actor.PersonalInfo.Passport.FIO.Patronymic}"
                            .Contains(value))!;
                OnPropertyChanged();
            }
        }

        public override string Path { get; set; } = "Actors";

        public Action Close;

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

        public ActorsViewModel()
        {
            (_, EmployeesPermissions, OrganisationsPermissions, UsersPermissions, PostersImagesPermissions) =
                UserGlobal.GetPermissions();
            Task.Run(async () => Actors = await Actors.GetAll());
            OpenDetails = () =>
            {
                SelectedActor.OpenThis?.Invoke();
                this.Close?.Invoke();
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DelegateCommand _addActor;

        public DelegateCommand AddActor
        {
            get
            {
                return _addActor ??= new(async _ =>
                {
                    ActorView actorView = new ActorView()
                    {
                        DataContext = new Actor()
                        {
                            PersonalInfo = new PersonalInformation()
                            {
                                Contact = new ContactDetails(),
                                Passport = new PassportDetails()
                                {
                                    FIO = new FullName(),
                                    DateOfBirth = DateTime.Now.AddDays(-1),
                                    DateOfIssue = DateTime.Now
                                }
                            },
                            Contr = new Contract()
                            {
                                DateOfCompletion = DateTime.Now.AddDays(1),
                                DateOfConclusion = DateTime.Now
                            }
                        }
                    };
                    (actorView.DataContext as Actor)!.Close = actorView.Close;
                    actorView.Show();
                    this.Close?.Invoke();
                });
            }
        }

        private DelegateCommand _updateActors;

        public DelegateCommand UpdateActors
        {
            get { return _updateActors ??= new(async _ => { Actors = await Actors.GetAll(); }); }
        }

        private DelegateCommand? _deleteActor;

        public DelegateCommand DeleteActor
        {
            get
            {
                return _deleteActor ??= new(async _ =>
                {
                    if (this._selectedActor != null && await this._selectedActor.Delete())
                        Actors.Remove(_selectedActor!);
                    else MessageBox.Show("Ошибка при удалении.");
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

        public Action OpenDetails;
    }
}