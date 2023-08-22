using EDLpakse.DialogBox;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;

namespace EDLpakse.WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();
      
        private void txtPass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (Pass.Password == "" || txtUser.Text == "")
                {
                    return;
                }
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    var Iuser = from h in db.T_Auths
                                where h.User_ID == txtUser.Text && h.User_Passward == Pass.Password
                                select h;

                    if (Iuser.Count() == 0)
                    {
                        NotFoundDialog frm = new NotFoundDialog();
                        frm.label1.Text = " ຊື່ຜູ່ໃຊ້ ຫຼີ ລະຫັດບໍຖືກຕ້ອງ ";
                        frm.ShowDialog();
                        Pass.Password = null;
                    }
                    else if (Iuser.Count() == 1)
                    {
                       
                        GlobalVariableClass.LevelAuth = Iuser.FirstOrDefault().User_Level;
                        GlobalVariableClass.UserAuth = Iuser.FirstOrDefault().User_ID;

                        MainWindow frm = new MainWindow();
                        frm.Show();
                        this.Hide();

                    }
                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void passwordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (Pass.Password == "" || txtUser.Text == "")
                {
                    return;
                }
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    var Iuser = from h in db.T_Auths
                                where h.User_ID == txtUser.Text && h.User_Passward == Pass.Password
                                select h;

                    if (Iuser.Count() == 0)
                    {
                        NotFoundDialog frm = new NotFoundDialog();
                        frm.label1.Text = " ຊື່ຜູ່ໃຊ້ ຫຼີ ລະຫັດບໍຖືກຕ້ອງ ";
                        frm.ShowDialog();
                        Pass.Password = null;
                    }
                    else if (Iuser.Count() == 1)
                    {

                        GlobalVariableClass.LevelAuth = Iuser.FirstOrDefault().User_Level;
                        GlobalVariableClass.UserAuth = Iuser.FirstOrDefault().User_ID;

                        MainWindow frm = new MainWindow();
                        frm.Show();
                        this.Hide();

                    }
                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
            Pass.Password = txtPass.Text;
        }
    }
}
