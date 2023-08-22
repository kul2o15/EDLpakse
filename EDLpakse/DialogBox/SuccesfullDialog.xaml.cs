using System.Windows;

namespace EDLpakse.DialogBox
{
    /// <summary>
    /// Interaction logic for SuccesfullDialog.xaml
    /// </summary>
    public partial class SuccesfullDialog : Window
    {
        public SuccesfullDialog()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OK.Focus();
        }
    }
}
