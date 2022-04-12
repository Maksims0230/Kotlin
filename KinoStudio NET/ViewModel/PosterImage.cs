using KinoStudio_NET.Annotations;
using KinoStudio_NET.Commands;
using KinoStudio_NET.Models;
using KinoStudio_NET.Models.Generics;
using KinoStudio_NET.ViewModel.Abstracts;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace KinoStudio_NET.ViewModel
{
    internal class PosterImage : ViewModelAbstract, INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private byte[] _value;
        private BitmapImage _bitmapImg;

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

        [JsonProperty(nameof(Value))]
        public byte[] Value
        {
            get => _value;
            set
            {
                if (value.Length == 0) return;
                _value = value;
                var image = new BitmapImage();
                using (var mem = new MemoryStream(value))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                BitmapImg = image;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public BitmapImage BitmapImg
        {
            get => _bitmapImg;
            set
            {
                _bitmapImg = value;
                OnPropertyChanged();
            }
        }

        public override string Path { get; set; } = "PastersImages";

        public PosterImage()
        {
        }

        [JsonIgnore] public bool Status;

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual async void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!Status && !this.IsNullOrDefault())
            {
                Status = true;
                return;
            }

            if (!Status || this.IsNullOrDefault()) return;

            try
            {
                if (propertyName != nameof(Id))
                {
                    if (await this.Exist())
                    {
                        await this.Update();
                    }
                    else
                    {
                        this.Id = (await this.Add()).Id;
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private bool IsNullOrDefault() => Name == default! || Value == default!;


        private DelegateCommand _addImage;

        [JsonIgnore]
        public DelegateCommand AddImage
        {
            get => _addImage ??= new(async _ => { (Name, Value) = await PosterImageModel.SelectImage(); });
        }
    }
}