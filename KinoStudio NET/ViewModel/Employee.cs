using KinoStudio_NET.Annotations;
using KinoStudio_NET.Commands;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.View;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace KinoStudio_NET.ViewModel
{
    internal class Employee : ViewModelAbstract, INotifyPropertyChanged
    {
        private int _id;
        private int _personalInformationId;
        private int _agreementId;
        private PersonalInformation _personalInformation;
        private Agreement _agreement;
        private Position _position;

        [JsonProperty("id")]
        public override int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("personalInformationId")]
        public int PersonalInformationId
        {
            get => _personalInformationId;
            set
            {
                _personalInformationId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("agreementId")]
        public int AgreementId
        {
            get => _agreementId;
            set
            {
                _agreementId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("personalInfo")]
        public PersonalInformation PersonalInfo
        {
            get => _personalInformation;
            set
            {
                _personalInformation = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("argee")]
        public Agreement Agree
        {
            get => _agreement;
            set
            {
                _agreement = value;
                Task.Run(async () => Pos = await new Position() { Id = Agree.PositionId }.Get());
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public Position Pos
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public Action OpenThis;

        [JsonIgnore] public override string Path { get; set; } = "Emloyees";

        public Employee()
        {
            OpenThis = () =>
            {
                EmployeeView employeeView = new EmployeeView() { DataContext = this };
                employeeView.Show();
            };
        }

        [JsonIgnore] public bool Status { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsNullOrDefault() => PersonalInfo == default! || Agree == default! ||
                                          PersonalInfo.IsNullOrDefault() || Agree.IsNullOrDefault();

        private bool IsValid() => PersonalInfo.IsValid() && Agree.IsValid();

        private DelegateCommand _back;

        [JsonIgnore]
        public DelegateCommand Back
        {
            get
            {
                return _back ??= new(_ =>
                {
                    EmployeesView employeesView = new EmployeesView();
                    this.Close?.Invoke();
                    employeesView.Show();
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
                            EmployeesView employeesView = new EmployeesView();
                            this.Close?.Invoke();
                            employeesView.Show();
                        }
                        else MessageBox.Show("Произошла ошибка во время сохранения.");
                    }
                    else MessageBox.Show("Заполнены не все или есть неверно заполненные поля.");
                });
            }
        }

        [JsonIgnore] private Action? _close;

        [JsonIgnore]
        public Action? Close
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