using KinoStudio_NET.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KinoStudio_NET.View
{
    /// <summary>
    /// Interaction logic for EmployeesView.xaml
    /// </summary>
    public partial class EmployeesView : Window
    {
        public EmployeesView()
        {
            InitializeComponent();
            (this.DataContext as EmployeesViewModel)!.Close = this.Close;
        }

        private void EmployeesDg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDg.SelectedItem is not null)
                EmployeesDg.ScrollIntoView(EmployeesDg.SelectedItem);
        }

        private void EmployeesDg_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as EmployeesViewModel)?.OpenDetails?.Invoke();
        }
    }
}