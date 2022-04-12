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
    internal class EmployeesViewModel : ViewModelsAbstract<Employee>, INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employees = new();
        private Employee _selectedEmployee;
        private string _searchLine = "";
        private bool _actorsPermission;
        private bool _organisationsPermission;
        private bool _usersPermission;
        private bool _postersImagesPermission;

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (Equals(value, _selectedEmployee)) return;
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        public string SearchLine
        {
            get => _searchLine;
            set
            {
                _searchLine = value;
                this.SelectedEmployee = string.IsNullOrWhiteSpace(_searchLine)
                    ? null!
                    : this.Employees.FirstOrDefault(pi =>
                        $"{pi.PersonalInfo.Passport.FIO.Surname} {pi.PersonalInfo.Passport.FIO.Name} {pi.PersonalInfo.Passport.FIO.Patronymic}"
                            .Contains(value))!;
                OnPropertyChanged();
            }
        }

        public override string Path { get; set; } = "Emloyees";

        public Action Close;

        public bool ActorsPermissions
        {
            get => _actorsPermission;
            set
            {
                _actorsPermission = value;
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

        public EmployeesViewModel()
        {
            (ActorsPermissions, _, OrganisationsPermissions, UsersPermissions, PostersImagesPermissions) =
                UserGlobal.GetPermissions();
            Task.Run(async () => Employees = await Employees.GetAll());
            OpenDetails = () =>
            {
                SelectedEmployee.OpenThis?.Invoke();
                this.Close?.Invoke();
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DelegateCommand _addEmployee;

        public DelegateCommand AddEmployee
        {
            get
            {
                return _addEmployee ??= new(async _ =>
                {
                    EmployeeView employeeView = new EmployeeView()
                    {
                        DataContext = new Employee()
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
                            Agree = new Agreement()
                            {
                                DateOfCompletion = DateTime.Now.AddDays(1),
                                DateOfConclusion = DateTime.Now
                            }
                        }
                    };
                    (employeeView.DataContext as Employee)!.Close = employeeView.Close;
                    employeeView.Show();
                    this.Close?.Invoke();
                });
            }
        }

        private DelegateCommand _updateEmployee;

        public DelegateCommand UpdateEmployees
        {
            get { return _updateEmployee ??= new(async _ => { Employees = await Employees.GetAll(); }); }
        }

        private DelegateCommand? _deleteEmployee;

        public DelegateCommand DeleteEmployee
        {
            get
            {
                return _deleteEmployee ??= new(async _ =>
                {
                    if (this._selectedEmployee != null && await this._selectedEmployee.Delete())
                        Employees.Remove(_selectedEmployee!);
                    else MessageBox.Show("Ошибка при удалении.");
                });
            }
        }
        
        private DelegateCommand? _showDiagram;

        public DelegateCommand ShowDiagram
        {
            get
            {
                return _showDiagram ??= new(async _ =>
                {
                    if (Employees.Count > 0)
                    {
                        EmployeeGraphView employeeGraphView = new EmployeeGraphView();
                        (employeeGraphView.DataContext as EmployeeGraphViewModel)!.Employees = this;
                        employeeGraphView.Show();
                    }
                });
            }
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