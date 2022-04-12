namespace KinoStudio_NET.ViewModel.Abstracts
{
    public abstract class ViewModelsAbstract<TObj> where TObj : ViewModelAbstract
    {
        public abstract string Path { get; set; }
    }
}