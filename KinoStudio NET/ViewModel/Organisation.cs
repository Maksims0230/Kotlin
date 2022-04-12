using KinoStudio_NET.Annotations;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KinoStudio_NET.ViewModel
{
    internal class Organisation : ViewModelAbstract, INotifyPropertyChanged
    {
        private int _id = 0;
        private string _name;
        private string _adress;
        private string _INN = "0";
        private bool _isReadOnlyFiels = false;

        [JsonProperty("OrganisationId")]
        public override int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Name))]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(Adress))]
        public string Adress
        {
            get => _adress;
            set
            {
                _adress = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(INN))]
        public string INN
        {
            get => _INN;
            set
            {
                if (value.All(char.IsDigit))
                    _INN = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public override string Path { get; set; } = "Organisations";

        [JsonIgnore]
        public bool IsReadOnlyLines
        {
            get => _isReadOnlyFiels;
            set
            {
                _isReadOnlyFiels = value;
                OnPropertyChanged();
            }
        }

        public Organisation()
        {
        }

        [JsonIgnore] public bool Status { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual async void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(Id) && propertyName != nameof(IsReadOnlyLines) && this._id != 0)
                IsReadOnlyLines = true;

            if (propertyName != nameof(Id) && propertyName != nameof(IsReadOnlyLines))
            {
                if (!Status && !this.IsNullOrDefault())
                {
                    Status = true;
                    return;
                }

                if (!Status || this.IsNullOrDefault()) return;

                if (await this.Exist())
                    await this.Update();
                else
                {
                    this.Id = (await this.Add()).Id;
                }
            }
        }

        private bool IsNullOrDefault() => this == default! || Name == default! || Adress == default! ||
                                          INN == default! || INN.Length < 10;
    }
}