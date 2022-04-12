using KinoStudio_NET.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace KinoStudio_NET.View
{
    /// <summary>
    /// Interaction logic for OrganisationsView.xaml
    /// </summary>
    public partial class OrganisationsView : Window
    {
        public OrganisationsView()
        {
            InitializeComponent();
            (this.DataContext as OrganisationsViewModel)!.Close = this.Close;
        }

        private void OrganisationsDg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrganisationsDg.SelectedItem is not null)
                OrganisationsDg.ScrollIntoView(OrganisationsDg.SelectedItem);
        }
    }
}