using KinoStudio_NET.Annotations;
using KinoStudio_NET.Models.Generics;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace KinoStudio_NET.ViewModel
{
    internal class Agreement : INotifyPropertyChanged
    {
        private int _id;
        private string _number;
        private int _organisationId;
        private DateTime _dateOfConclusion;
        private DateTime _dateOfCompletion;
        private int _scheduleId;
        private int _positionId;
        private float _salary;
        private ObservableCollection<Organisation> _organisations;
        private ObservableCollection<Schedule> _schedules;
        private ObservableCollection<Position> _positions;


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

        [NotNull]
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

        [JsonProperty(nameof(OrganisationId))]
        public int OrganisationId
        {
            get => _organisationId;
            set
            {
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

        [JsonProperty(nameof(ScheduleId))]
        public int ScheduleId
        {
            get => _scheduleId;
            set
            {
                _scheduleId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(PositionId))]
        public int PositionId
        {
            get => _positionId;
            set
            {
                _positionId = value;
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
        public ObservableCollection<Schedule> Schedules
        {
            get => _schedules;
            set
            {
                _schedules = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<Position> Positions
        {
            get => _positions;
            set
            {
                _positions = value;
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

        [JsonIgnore]
        public Schedule? SelectedSchedule
        {
            get => Schedules?.FirstOrDefault(schedule => schedule.Id == ScheduleId) ?? null;
            set
            {
                if (ScheduleId != value!.Id)
                    ScheduleId = value!.Id;
            }
        }

        [JsonIgnore]
        public Position? SelectedPosition
        {
            get => Positions?.FirstOrDefault(schedule => schedule.Id == ScheduleId) ?? null;
            set
            {
                if (PositionId != value!.Id)
                    PositionId = value!.Id;
            }
        }

        public Agreement()
        {
            Parallel.Invoke(async () =>
                    Organisations = await Organisations.GetAll(),
                async () =>
                    Schedules = await Schedules.GetAll(),
                async () =>
                    Positions = await Positions.GetAll()
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsNullOrDefault() => Number == default! || DateOfConclusion == default ||
                                         DateOfCompletion == default || Salary == 0f;

        public bool IsValid() =>
            DateOfConclusion.Date >= DateTime.Now.Date && DateOfCompletion.Date > DateTime.Now.Date;
    }
}