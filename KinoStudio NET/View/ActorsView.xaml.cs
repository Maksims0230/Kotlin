using KinoStudio_NET.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KinoStudio_NET.View
{
    /// <summary>
    /// Interaction logic for ActorsView.xaml
    /// </summary>
    public partial class ActorsView : Window
    {
        public ActorsView()
        {
            InitializeComponent();
            (this.DataContext as ActorsViewModel)!.Close = this.Close;
        }

        private void ActorsDg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActorsDg.SelectedItem is not null)
                ActorsDg.ScrollIntoView(ActorsDg.SelectedItem);
        }

        private void ActorsDg_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as ActorsViewModel)?.OpenDetails?.Invoke();
        }
    }
}