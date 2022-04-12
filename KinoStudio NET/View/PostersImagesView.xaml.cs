using KinoStudio_NET.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace KinoStudio_NET.View
{
    /// <summary>
    /// Interaction logic for PostersImagesView.xaml
    /// </summary>
    public partial class PostersImagesView : Window
    {
        public PostersImagesView()
        {
            InitializeComponent();
            (this.DataContext as PostersImagesViewModel)!.Close = this.Close;
        }

        private void PostersImagesDg_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PostersImagesDg.SelectedItem is not null)
                PostersImagesDg.ScrollIntoView(PostersImagesDg.SelectedItem);
        }
    }
}