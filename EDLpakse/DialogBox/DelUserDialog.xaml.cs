using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EDLpakse.DialogBox
{
    /// <summary>
    /// Interaction logic for DelUserDialog.xaml
    /// </summary>
    public partial class DelUserDialog : Window
    {
        public DelUserDialog()
        {
            InitializeComponent();
        }
        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                T_Auth ulog = new T_Auth();
                ulog.User_ID = comboBoxUser.Text;
                db.T_Auths.DeleteOnSubmit(ulog);
                db.SubmitChanges();
                this.DialogResult = true;
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                comboBoxUser.ItemsSource = from h in db.T_Auths
                                           where h.User_Level != "2" && h.User_Level != "5"
                                           select h.User_ID;
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
