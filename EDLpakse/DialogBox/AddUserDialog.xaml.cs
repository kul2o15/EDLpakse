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
    /// Interaction logic for AddUserDialog.xaml
    /// </summary>
    public partial class AddUserDialog : Window
    {
        public AddUserDialog()
        {
            InitializeComponent();
        }
        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUser.Focus();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Pass.Visibility = System.Windows.Visibility.Hidden;
            txtPass.Visibility = System.Windows.Visibility.Visible;
            txtPass.Text = Pass.Password;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Pass.Visibility = System.Windows.Visibility.Visible;
            txtPass.Visibility = System.Windows.Visibility.Hidden;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lv = "";
                switch (comboBoxAuth.Text)
                {
                    case "ພະນັກງານທົ່ວໄປ":
                        lv = "1";
                        break;
                    case "ພະນັກງານຜູ້ບັນທຶກຂໍ້ມູນ":
                        lv = "2";
                        break;                  
                }

                var Iuser = from h in db.T_Auths
                            where h.User_ID == txtUser.Text
                            select h;

                if (Iuser.Count() == 1)
                {
                    NotFoundDialog frm = new NotFoundDialog();
                    frm.label1.Text = " ຊື່ຜູ່ໃຊ້ມີແລ້ວ ";
                    frm.ShowDialog();
                    Pass.Password = null;
                }
                else
                {
                    T_Auth ulog = new T_Auth();
                    ulog.User_ID = txtUser.Text;
                    ulog.User_Level = lv;

                    if (checkBox.IsChecked == true)
                    {
                        ulog.User_Passward = txtPass.Text;
                    }
                    else
                    {
                        ulog.User_Passward = Pass.Password;
                    }

                    db.T_Auths.InsertOnSubmit(ulog);
                    db.SubmitChanges();
                }

                this.DialogResult = true;

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
