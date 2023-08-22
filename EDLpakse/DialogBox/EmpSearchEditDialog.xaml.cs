using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EDLpakse.DialogBox
{
    /// <summary>
    /// Interaction logic for EmpSearchEditDialog.xaml
    /// </summary>
    public partial class EmpSearchEditDialog : Window
    {
        public EmpSearchEditDialog()
        {
            InitializeComponent();
        }
      
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariableClass.TempString = comboBoxTemp.Text;
            this.DialogResult = true;
        }
    }
}
