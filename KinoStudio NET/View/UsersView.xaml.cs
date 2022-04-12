using System.Windows;
using System.Windows.Controls;
using KinoStudio_NET.ViewModel;

namespace KinoStudio_NET.View
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : Window
    {
        public UsersView()
        {
            InitializeComponent();
            (this.DataContext as UsersViewModel)!.Close = this.Close;
        }

        private void RoleSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            this.UsersDg.SelectedItem = null;

        private void UsersDg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDg.SelectedItem is not null) 
                UsersDg.ScrollIntoView(UsersDg.SelectedItem);
        }
    }
}