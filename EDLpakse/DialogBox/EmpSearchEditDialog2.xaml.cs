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
    /// Interaction logic for EmpSearchEditDialog2.xaml
    /// </summary>
    public partial class EmpSearchEditDialog2 : Window
    {
        public EmpSearchEditDialog2()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariableClass.TempString = txtSearch.Text;
            this.DialogResult = true;
        }
     
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                GlobalVariableClass.TempString = txtSearch.Text;
                this.DialogResult = true;
            }
        }
    }
}
