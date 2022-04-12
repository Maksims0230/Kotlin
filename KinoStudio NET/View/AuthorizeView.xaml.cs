using KinoStudio_NET.ViewModel;
using System.Windows;

namespace KinoStudio_NET.View
{
    /// <summary>
    /// Interaction logic for AuthorizeView.xaml
    /// </summary>
    public partial class AuthorizeView : Window
    {
        public AuthorizeView()
        {
            InitializeComponent();

            if (this.DataContext is AuthorizeViewModel vm)
                vm.CloseAction = this.Close;
        }
    }
}