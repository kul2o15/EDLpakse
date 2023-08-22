using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace EDLpakse.DialogBox
{
    /// <summary>
    /// Interaction logic for InsertActivityDialog.xaml
    /// </summary>
    public partial class InsertActivityDialog : Window
    {
        public InsertActivityDialog()
        {
            InitializeComponent();
        }
        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
         
            try
            {
                
                T_Activity tac = new T_Activity();
                tac.Activity = txtName.Text + " " + comboBoxDay.Text + "/" + comboBoxMonth.Text + "/" + comboBoxYear.Text;
                tac.Activity_Detail = txtNote.Text;
                tac.Activity_When = comboBoxDay.Text + "/" + comboBoxMonth.Text + "/" + comboBoxYear.Text;
                tac.Activity_End = comboBoxEDay.Text + "/" + comboBoxEMonth.Text + "/" + comboBoxEYear.Text;

                db.T_Activities.InsertOnSubmit(tac);
                db.SubmitChanges();

                this.DialogResult = true;
           
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmDialog frm = new ConfirmDialog();
                frm.label1.Text = "ຕ້ອງການລົບຂໍ້ມູນນີ້ແທ້ ຫຼື ບໍ່";
                frm.ShowDialog();

                if (frm.DialogResult == true)
                {

                    DataRowView drv = (DataRowView)dataGrid.SelectedItem;
                    String acID = (drv["ລະຫັດ"]).ToString();

                    var deleteOrderDetails = from ac in db.T_Activities where ac.Activity_ID.ToString() == acID select ac;

                    foreach (var detail in deleteOrderDetails)
                    {
                        db.T_Activities.DeleteOnSubmit(detail);
                    }

                    db.SubmitChanges();

                    var iAc = from AC in db.T_Activities
                              select new
                              {
                                  AC.Activity_ID,
                                  AC.Activity
                              };
                    ReportDataSet.ActivityTempDataTable dt = new ReportDataSet.ActivityTempDataTable();

                    foreach (var data in iAc.AsEnumerable())
                    {
                        dt.Rows.Add(data.Activity_ID, data.Activity);
                    }

                    dataGrid.ItemsSource = dt.DefaultView;

                }

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
                               
                    var iAc = from AC in db.T_Activities                 
                               select new
                               {
                                   AC.Activity_ID,
                                   AC.Activity
                               };
                    ReportDataSet.ActivityTempDataTable dt = new ReportDataSet.ActivityTempDataTable();

                    foreach (var data in iAc.AsEnumerable())
                    {
                        dt.Rows.Add(data.Activity_ID,data.Activity );
                    }

                    dataGrid.ItemsSource = dt.DefaultView;
              
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
