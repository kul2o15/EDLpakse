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
    /// Interaction logic for VacationDialog.xaml
    /// </summary>
    public partial class VacationDialog : Window
    {
        public VacationDialog()
        {
            InitializeComponent();
        }

        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboBoxVacDay.Text != "0")
                {
                    T_Vacation_Info vac = new T_Vacation_Info();

                    var newEmp = from em in db.T_Employees
                                 where em.Employee_ID.ToString() == txtidemp.Text
                                 select em;
                    int newvac = Convert.ToInt32(newEmp.FirstOrDefault().Vacation) - Convert.ToInt32(comboBoxVacDay.Text);

                    foreach (T_Employee emp in newEmp)
                    {

                        emp.Vacation = newvac.ToString();

                    }

                    vac.Employee_ID = Convert.ToInt32(txtidemp.Text);
                    vac.Vacation_Start = comboBoxDay.Text + "/" + comboBoxMonth.Text + "/" + comboBoxYear.Text;
                    vac.Vacation_End = comboBoxEDay.Text + "/" + comboBoxEMonth.Text + "/" + comboBoxEYear.Text;
                    vac.Vacation_Info = txtNote.Text;
                    vac.Vacation_Howlong = comboBoxVacDay.Text + " ວັນ";

                    db.T_Vacation_Infos.InsertOnSubmit(vac);
                    db.SubmitChanges();
                 
                }

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
                dataGrid.ItemsSource = from va in db.T_Vacation_Infos
                                       where va.Employee_ID.ToString() == txtidemp.Text
                                       select new { ລາພັກ = va.Vacation_Howlong, ວັນທີ່ = va.Vacation_Start,
                                                    ຫາວັນທີ່ = va.Vacation_End, ເຫດຜົນ = va.Vacation_Info};
            }


            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
