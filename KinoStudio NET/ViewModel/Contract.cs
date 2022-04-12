using KinoStudio_NET.Annotations;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace KinoStudio_NET.ViewModel
{
    internal class Contract : ViewModelAbstract, INotifyPropertyChanged
    {
        private int _id;
        private string _contractNumber;
        private int _organisationId;
        private DateTime _dateOfConclusion;
        private DateTime _dateOfCompletion;
        private float _salary;
        private ObservableCollection<Organisation> _organisations;

        [JsonProperty("ID")]
        public override int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ContractNumber))]
        public string ContractNumber
        {
            get => _contractNumber;
            set
            {
                _contractNumber = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(OrganisationId))]
        public int OrganisationId
        {
            get => _organisationId;
            set
            {
                if (value == _organisationId) return;
                _organisationId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(DateOfConclusion))]
        public DateTime DateOfConclusion
        {
            get => _dateOfConclusion;
            set
            {
                _dateOfConclusion = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(DateOfCompletion))]
        public DateTime DateOfCompletion
        {
            get => _dateOfCompletion;
            set
            {
                _dateOfCompletion = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Salary))]
        public float Salary
        {
            get => _salary;
            set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<Organisation> Organisations
        {
            get => _organisations;
            set
            {
                _organisations = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public Organisation? SelectedOrganisation
        {
            get => Organisations?.FirstOrDefault(organ => organ.Id == OrganisationId) ?? null;
            set
            {
                if (OrganisationId != value!.Id)
                    OrganisationId = value!.Id;
            }
        }

        [JsonIgnore] public override string Path { get; set; } = "Contracts";

        public Contract()
        {
            Task.Run(async () => Organisations = await Organisations.GetAll());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsNullOrDefault() => ContractNumber == default! || DateOfConclusion == default ||
                                         DateOfCompletion == default || Salary == 0f;

        public bool IsValid() =>
            DateOfConclusion.Date >= DateTime.Now.Date && DateOfCompletion.Date > DateTime.Now.Date;
    }
}