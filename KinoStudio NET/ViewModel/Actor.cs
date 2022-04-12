using KinoStudio_NET.Annotations;
using KinoStudio_NET.Commands;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.View;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KinoStudio_NET.ViewModel
{
    internal class Actor : ViewModelAbstract, INotifyPropertyChanged
    {
        private int _id;
        private int _personalInformationId;
        private int _contractId;
        private PersonalInformation _personalInformation;
        private Contract _contract;

        [JsonProperty("ID")]
        public override int Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(PersonalInformationId))]
        public int PersonalInformationId
        {
            get => _personalInformationId;
            set
            {
                if (value == _personalInformationId) return;
                _personalInformationId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ContractId))]
        public int ContractId
        {
            get => _contractId;
            set
            {
                if (value == _contractId) return;
                _contractId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(PersonalInfo))]
        public PersonalInformation PersonalInfo
        {
            get => _personalInformation;
            set
            {
                if (Equals(value, _personalInformation)) return;
                _personalInformation = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Contr))]
        public Contract Contr
        {
            get => _contract;
            set
            {
                _contract = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public Action OpenThis;

        [JsonIgnore] public override string Path { get; set; } = "Actors";

        public Actor()
        {
            OpenThis = () =>
            {
                ActorView actorView = new ActorView() {DataContext = this};
                actorView.Show();
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsNullOrDefault() => PersonalInfo == default! || Contr == default! ||
                                          PersonalInfo.IsNullOrDefault() || Contr.IsNullOrDefault();

        private bool IsValid() => PersonalInfo.IsValid() && Contr.IsValid();

        private DelegateCommand _back;

        [JsonIgnore]
        public DelegateCommand Back
        {
            get
            {
                return _back ??= new(_ =>
                {
                    ActorsView employeesView = new ActorsView();
                    employeesView.Show();
                    this.Close?.Invoke();
                });
            }
        }

        private DelegateCommand _save;

        [JsonIgnore]
        public DelegateCommand Save
        {
            get
            {
                return _save ??= new(async _ =>
                {
                    if (!this.IsNullOrDefault() && this.IsValid())
                    {
                        var result = Id == 0 ? await this.Add() : await this.Update();
                        if (result != null!)
                        {
                            ActorsView actorsView = new ActorsView();
                            actorsView.Show();
                            this.Close?.Invoke();
                        }
                        else MessageBox.Show("Произошла ошибка во время сохранения.");
                    }
                    else MessageBox.Show("Заполнены не все или есть неверно заполненные поля.");
                });
            }
        }

        [JsonIgnore] private Action _close;

        [JsonIgnore]
        public Action Close
        {
            get => _close;
            set
            {
                _close = value;
                OnPropertyChanged();
            }
        }
    }
}