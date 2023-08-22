using EDLpakse.DialogBox;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace EDLpakse.WPF
{
    /// <summary>
    /// Interaction logic for ActivityWindow.xaml
    /// </summary>
    public partial class ActivityWindow : Window
    {
        public ActivityWindow()
        {
            InitializeComponent();
        }

        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();
        int IndexSearch;
        private void Window_Closed(object sender, System.EventArgs e)
        {
            MainWindow frm = new MainWindow();
            frm.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow frm = new MainWindow();
            frm.Show();
            this.Hide();
        }
                
        private void BtnDepart_Click(object sender, RoutedEventArgs e)
        {

            try
            {
               
                EmpSearchEditDialog frm = new EmpSearchEditDialog();
                frm.label1.Text = "ເລືອກພະແນກທີ່ຕ້ອງການຊອກຫາ";
                frm.comboBoxTemp.ItemsSource = (from d in db.T_Departments select d.Departmen).ToList();
                frm.ShowDialog();
                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {
                   
                    IndexSearch = 1;
                    texttitle.Text = "ພະແນກ " + GlobalVariableClass.TempString;
                    var iEmp = from Emp in db.T_Employees           
                               where Emp.Department_ID == (from d in db.T_Departments where d.Departmen == GlobalVariableClass.TempString select d.Department_ID).FirstOrDefault()
                               select new
                               {
                                   Emp.Employee_ID,
                                   Emp.Titile_ID,
                                   Emp.FirstName,
                                   Emp.LastName
                               };
                    ReportDataSet.ActivityInputDataTable dt = new ReportDataSet.ActivityInputDataTable();

                    foreach (var data in iEmp.AsEnumerable())
                    {
                        dt.Rows.Add(data.Employee_ID, (from t in db.T_Titles where t.Title_ID == data.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data.FirstName + " " + data.LastName);
                    }

                    dataGrid.ItemsSource = dt.DefaultView;
                    Search();

                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnUnit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmpSearchEditDialog frm = new EmpSearchEditDialog();
                frm.label1.Text = "ເລືອກໜ່ວຍງານທີ່ຕ້ອງການຊອກຫາ";
                frm.comboBoxTemp.ItemsSource = (from d in db.T_Units select d.Unit).ToList();
                frm.ShowDialog();
                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {

                    IndexSearch = 2;
                    texttitle.Text = "ໜ່ວຍງານ " + GlobalVariableClass.TempString;
                    var iEmp = from Emp in db.T_Employees
                               where Emp.Unit_ID == (from d in db.T_Units where d.Unit == GlobalVariableClass.TempString select d.Unit_ID).FirstOrDefault()
                               select new
                               {
                                   Emp.Employee_ID,
                                   Emp.Titile_ID,
                                   Emp.FirstName,
                                   Emp.LastName
                               };
                    ReportDataSet.ActivityInputDataTable dt = new ReportDataSet.ActivityInputDataTable();

                    foreach (var data in iEmp.AsEnumerable())
                    {
                        dt.Rows.Add(data.Employee_ID, (from t in db.T_Titles where t.Title_ID == data.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data.FirstName + " " + data.LastName);
                    }

                    dataGrid.ItemsSource = dt.DefaultView;
                    Search();
                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                DataRowView drv = (DataRowView)dataGrid.SelectedItem;
                int empID = Convert.ToInt32(drv["ລະຫັດພະນັກງານ"]);

                var newEmp = from em in db.T_Participates
                             where em.Employee_ID == empID && em.Activity_ID == 
                             (from ac in db.T_Activities where ac.Activity == GlobalVariableClass.TempString3 select ac.Activity_ID).FirstOrDefault()
                             select em.Employee_ID;
              
                if (newEmp.Count() != 1)
                {
                    T_Participate pa = new T_Participate();

                    pa.Activity_ID = (from ac in db.T_Activities where ac.Activity == GlobalVariableClass.TempString3 select ac.Activity_ID).FirstOrDefault();                  
                    pa.Employee_ID = empID;

                    db.T_Participates.InsertOnSubmit(pa);
                    db.SubmitChanges();
                    Search();
                }
               
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataRowView drv = (DataRowView)dataGrid2.SelectedItem;
                int empID = Convert.ToInt32(drv["ລະຫັດພະນັກງານ"]);

                var deleteOrderDetails = from pa3 in db.T_Participates
                                         where pa3.Employee_ID == empID && pa3.Activity_ID == 
                                         (from ac in db.T_Activities where ac.Activity == GlobalVariableClass.TempString3 select ac.Activity_ID).FirstOrDefault()
                                         select pa3;

                foreach (var detail in deleteOrderDetails)
                {
                    db.T_Participates.DeleteOnSubmit(detail);
                }

                db.SubmitChanges();
                Search();
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Search()
        {
            try
            {

                #region 1
                if (IndexSearch == 1)
                {

                    var iEmp2 = from pa2 in db.T_Participates
                                join Emp2 in db.T_Employees on pa2.Employee_ID equals Emp2.Employee_ID into newpa
                                from npa in newpa.DefaultIfEmpty()
                                where npa.Department_ID == (from d in db.T_Departments where d.Departmen == GlobalVariableClass.TempString select d.Department_ID).FirstOrDefault()
                                && pa2.Activity_ID == (from ac in db.T_Activities where ac.Activity == GlobalVariableClass.TempString3 select ac.Activity_ID).FirstOrDefault()
                                select new
                                {
                                    pa2.Employee_ID,
                                    npa.Titile_ID,
                                    npa.FirstName,
                                    npa.LastName
                                };
                    ReportDataSet.ActivityInputDataTable dt2 = new ReportDataSet.ActivityInputDataTable();

                    foreach (var data2 in iEmp2.AsEnumerable())
                    {
                        dt2.Rows.Add(data2.Employee_ID, (from t in db.T_Titles where t.Title_ID == data2.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data2.FirstName + " " + data2.LastName);
                    }

                    dataGrid2.ItemsSource = dt2.DefaultView;

                }
                #endregion

                #region 2
                else if (IndexSearch == 2)
                {
                    var iEmp2 = from pa2 in db.T_Participates
                                join Emp in db.T_Employees on pa2.Employee_ID equals Emp.Employee_ID into newpa
                                from npa in newpa.DefaultIfEmpty()
                                where npa.Unit_ID == (from d in db.T_Units where d.Unit == GlobalVariableClass.TempString select d.Unit_ID).FirstOrDefault()
                                && pa2.Activity_ID == (from ac in db.T_Activities where ac.Activity == GlobalVariableClass.TempString3 select ac.Activity_ID).FirstOrDefault()
                                select new
                                {
                                    npa.Employee_ID,
                                    npa.Titile_ID,
                                    npa.FirstName,
                                    npa.LastName
                                };
                    ReportDataSet.ActivityInputDataTable dt2 = new ReportDataSet.ActivityInputDataTable();

                    foreach (var data in iEmp2.AsEnumerable())
                    {
                        dt2.Rows.Add(data.Employee_ID, (from t in db.T_Titles where t.Title_ID == data.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data.FirstName + " " + data.LastName);
                    }

                    dataGrid2.ItemsSource = dt2.DefaultView;

                }
                #endregion
              
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
