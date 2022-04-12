using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KinoStudio_NET.Annotations;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;

namespace KinoStudio_NET.ViewModel
{
    internal class Schedule : ViewModelAbstract, INotifyPropertyChanged
    {
        private int _id = 0;
        private string _schedule1;

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

        [JsonProperty("Schedule1")]
        public string Schedule1
        {
            get => _schedule1;
            set
            {
                _schedule1 = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public override string Path { get; set; } = "Schedules";

        public Schedule()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsNullOrDefault() => this == default! || Id == default || Schedule1 == default!;
    }
}