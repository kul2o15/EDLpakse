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
    /// Interaction logic for ChangePassDialog.xaml
    /// </summary>
    public partial class ChangePassDialog : Window
    {
        public ChangePassDialog()
        {
            InitializeComponent();
        }
        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PassOld.Focus();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            PassOld.Visibility = System.Windows.Visibility.Hidden;
            PassNew.Visibility = System.Windows.Visibility.Hidden;
            txtPassold.Visibility = System.Windows.Visibility.Visible;
            txtPassnew.Visibility = System.Windows.Visibility.Visible;
            txtPassold.Text = PassOld.Password;
            txtPassnew.Text = PassNew.Password;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PassOld.Visibility = System.Windows.Visibility.Visible;
            PassNew.Visibility = System.Windows.Visibility.Visible;
            txtPassold.Visibility = System.Windows.Visibility.Hidden;
            txtPassnew.Visibility = System.Windows.Visibility.Hidden;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
             try
            {
                if (PassOld.Password == "" || PassNew.Password == "")
                {
                    return;
                }
               
                    var Iuser = from h in db.T_Auths
                                where h.User_ID == GlobalVariableClass.UserAuth && h.User_Passward == PassOld.Password
                                select h;

                    if (Iuser.Count() == 0)
                    {                      
                        NotFoundDialog frm = new NotFoundDialog();
                        frm.label1.Text = " ລະຫັດເກົ່າບໍຖືກຕ້ອງ ";
                        frm.ShowDialog();
                        PassOld.Password = null;
                    }
                    else if (Iuser.Count() == 1)
                    {
                        foreach (T_Auth ath in Iuser)
                        {
                        ath.User_Passward = PassNew.Password;
                        }

                    db.SubmitChanges();
                    this.DialogResult = true;
                    }
               

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
