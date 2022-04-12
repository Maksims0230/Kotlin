using KinoStudio_NET.Annotations;
using KinoStudio_NET.Commands;
using KinoStudio_NET.Globals;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.View;
using KinoStudio_NET.ViewModel.Abstracts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace KinoStudio_NET.ViewModel
{
    internal class PostersImagesViewModel : ViewModelsAbstract<PosterImage>, INotifyPropertyChanged
    {
        private ObservableCollection<PosterImage> _postersImages;
        private PosterImage _selectedPosterImage;
        private bool _actorsPermission;
        private bool _employeesPermission;
        private bool _organisationsPermission;
        private bool _usersPermission;

        public ObservableCollection<PosterImage> PostersImages
        {
            get => _postersImages;
            set
            {
                _postersImages = value;
                OnPropertyChanged();
            }
        }

        public PosterImage SelectedPosterImage
        {
            get => _selectedPosterImage;
            set
            {
                _selectedPosterImage = value;
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

        public PostersImagesViewModel()
        {
            (ActorsPermissions, EmployeesPermissions, OrganisationsPermissions, UsersPermissions, _) =
                UserGlobal.GetPermissions();
            Task.Run(async () => PostersImages = await PostersImages.GetAll());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string Path { get; set; } = "PastersImages";

        private DelegateCommand _addImage;

        public DelegateCommand AddImage
        {
            get => _addImage ??= new(_ => { PostersImages.Add(new PosterImage() {Status = true}); });
        }

        private DelegateCommand _addPosterImage;

        public DelegateCommand AddPosterImage
        {
            get
            {
                return _addPosterImage ??= new(async _ => { PostersImages.Add(new PosterImage() {Status = true}); });
            }
        }

        private DelegateCommand _updatePostersImages;

        public DelegateCommand UpdatePostersImages
        {
            get => _updatePostersImages ??= new(_ =>
            {
                Task.Run(async () => PostersImages = await PostersImages.GetAll());
            });
        }

        private DelegateCommand? _deletePosterImage;

        public DelegateCommand DeletePosterImage
        {
            get
            {
                return _deletePosterImage ??= new(async _ =>
                {
                    if (this._selectedPosterImage != null && await this._selectedPosterImage.Delete())
                        PostersImages.Remove(_selectedPosterImage!);
                    else MessageBox.Show("Ошибка при удалении.");
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
                return _toOrganisations ??= new(_ =>
                {
                    OrganisationsView organisationsView = new();
                    organisationsView.Show();
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

        public Action Close;
    }
}