using System.Windows;

namespace EDLpakse.DialogBox
{
    /// <summary>
    /// Interaction logic for NotFoundDialog.xaml
    /// </summary>
    public partial class NotFoundDialog : Window
    {
        public NotFoundDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            OK.Focus();
        }
    }
}
