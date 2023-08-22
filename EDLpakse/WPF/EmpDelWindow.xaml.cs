using EDLpakse.DialogBox;
using System;
using System.Windows;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Data;

namespace EDLpakse.WPF
{
    /// <summary>
    /// Interaction logic for EmpDelWindow.xaml
    /// </summary>
    public partial class EmpDelWindow : Window
    {
        public EmpDelWindow()
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

        private void BtnId_Click(object sender, RoutedEventArgs e)
        {
           
                EmpSearchEditDialog2 frm = new EmpSearchEditDialog2();
                frm.label1.Text = "ປ້ອນລະຫັດພະນັກງານທີ່ຕ້ອງການຊອກຫາ";
                frm.ShowDialog();
                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {

                    IndexSearch = 1;
                    texttitle.Text = "ລະຫັດ " + GlobalVariableClass.TempString;
                    Search();
                }
                          
        }

        private void BtnName_Click(object sender, RoutedEventArgs e)
        {
           
                EmpSearchEditDialog2 frm = new EmpSearchEditDialog2();
                frm.label1.Text = "ປ້ອນຊື່ພະນັກງານທີ່ຕ້ອງການຊອກຫາ";
                frm.ShowDialog();
                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {

                    IndexSearch = 2;
                    texttitle.Text = "ຊື່ " + GlobalVariableClass.TempString;
                    Search();
                }
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

                    IndexSearch = 3;
                    texttitle.Text = "ພະແນກ " + GlobalVariableClass.TempString;
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

                    IndexSearch = 4;
                    texttitle.Text = "ໜ່ວຍງານ " + GlobalVariableClass.TempString;
                    Search();
                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow frm = new MainWindow();
            frm.Show();
            this.Hide();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //if (GlobalVariableClass.LevelAuth != "1")
                //{            
                EDLpakseDataClassesDataContext db2 = new EDLpakseDataClassesDataContext();
                DataRowView drv = (DataRowView)dataGrid.SelectedItem;
                String empID = (drv["ລະຫັດພະນັກງານ"]).ToString();
                             
                var Iemp = (from h in db2.T_Employees
                               where h.Employee_ID.ToString() == empID
                               select h);

                #region Display
               
                EmpEditDialog frm = new EmpEditDialog();
                GlobalVariableClass.TempString2 = empID;

                frm.comboBoxTitleName.SelectedValue = (from t in db.T_Titles where t.Title_ID == Iemp.FirstOrDefault().Titile_ID select t.Title).FirstOrDefault();
                frm.txtName.Text = Iemp.FirstOrDefault().FirstName;
                frm.txtSurname.Text = Iemp.FirstOrDefault().LastName;
                frm.comboBoxCulture.SelectedValue = (from t in db.T_Cultures where t.Culture_ID == Iemp.FirstOrDefault().Culture_ID select t.Culture).FirstOrDefault();
                frm.comboBoxEducation.SelectedValue = (from t in db.T_Educations where t.Education_ID == Iemp.FirstOrDefault().Education_ID select t.Education).FirstOrDefault();
                frm.comboBoxFrom.SelectedValue = (from t in db.T_Countries where t.Country_ID == Iemp.FirstOrDefault().Country_ID select t.Country).FirstOrDefault();
                frm.comboBoxStatus.SelectedValue = (from t in db.T_Kind_staffs where t.Staff_ID == Iemp.FirstOrDefault().Staff_ID select t.Staff_Kind).FirstOrDefault();
                frm.comboBoxDay.Text = Iemp.FirstOrDefault().StartWork_Date.Split('/')[0];
                frm.comboBoxMonth.Text = Iemp.FirstOrDefault().StartWork_Date.Split('/')[1];
                frm.comboBoxYear.Text = Iemp.FirstOrDefault().StartWork_Date.Split('/')[2];
                frm.comboBoxUnit.SelectedValue = (from t in db.T_Units where t.Unit_ID == Iemp.FirstOrDefault().Unit_ID select t.Unit).FirstOrDefault();
                frm.comboBoxPosition.SelectedValue = (from t in db.T_Positions where t.Position_ID == Iemp.FirstOrDefault().Position_ID select t.Position).FirstOrDefault();
                frm.comboBoxDepartment.SelectedValue = (from t in db.T_Departments where t.Department_ID == Iemp.FirstOrDefault().Department_ID select t.Departmen).FirstOrDefault();
              
                frm.txtNote.Text = Iemp.FirstOrDefault().WorkExperial;

                if (Iemp.FirstOrDefault().EndWork_Date != null)
                {
                    frm.comboBoxEDay.Text = Iemp.FirstOrDefault().EndWork_Date.Split('/')[0];
                    frm.comboBoxEMonth.Text = Iemp.FirstOrDefault().EndWork_Date.Split('/')[1];
                    frm.comboBoxEYear.Text = Iemp.FirstOrDefault().EndWork_Date.Split('/')[2];
                }

                if (Iemp.FirstOrDefault().Photo != null)
                {
                    byte[] b = (Iemp.FirstOrDefault().Photo).ToArray();
                    MemoryStream memoryStream = new MemoryStream(b);
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = memoryStream;
                    imageSource.EndInit();
                    frm.image.Source = imageSource;
                }
                
                frm.ShowDialog();

                #endregion

                if (frm.DialogResult == true)
                {
                    Search();
                }

                //}
                //    else
                //    {
                //    NotFoundDialog frm3 = new NotFoundDialog();
                //    frm3.label1.Text = " ທ່ານບໍ່ສາມາດໃຊ້ສ່ວນນີ້ຂອງລະບົບ ";
                //    frm3.ShowDialog();
                //}

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
                    String empID = (drv["ລະຫັດພະນັກງານ"]).ToString();

                    var deleteOrderDetails = from em in db.T_Employees where em.Employee_ID.ToString() == empID select em;

                    foreach (var detail in deleteOrderDetails)
                    {
                       db.T_Employees.DeleteOnSubmit(detail);
                    }
          
                    db.SubmitChanges();

                    Search();

                }

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
                        var iEmp = from Emp in db.T_Employees
                                   where Emp.Employee_ID.ToString() == GlobalVariableClass.TempString
                                   select new
                                   {
                                       Emp.Employee_ID,
                                       Emp.Titile_ID,
                                       Emp.FirstName,
                                       Emp.LastName,
                                       Emp.Department_ID,
                                       Emp.Unit_ID,
                                       Emp.Position_ID,
                                       Emp.Vacation
                                   };
                        ReportDataSet.EmpSearchDataTable dt = new ReportDataSet.EmpSearchDataTable();

                        foreach (var data in iEmp.AsEnumerable())
                        {
                            dt.Rows.Add(data.Employee_ID, (from t in db.T_Titles where t.Title_ID == data.Titile_ID select t.Title).FirstOrDefault() + " "
                                + data.FirstName + " " + data.LastName, (from d in db.T_Departments where d.Department_ID == data.Department_ID select d.Departmen).FirstOrDefault(),
                                (from u in db.T_Units where u.Unit_ID == data.Unit_ID select u.Unit).FirstOrDefault(),
                                (from p in db.T_Positions where p.Position_ID == data.Position_ID select p.Position).FirstOrDefault(), data.Vacation + " ວັນ");
                        }

                        dataGrid.ItemsSource = dt.DefaultView;
                        textcount.Text = "";

                 }
                 #endregion

                #region 2
                else if (IndexSearch == 2)
                {
                    var iEmp = from Emp in db.T_Employees
                               where Emp.FirstName == GlobalVariableClass.TempString
                               select new
                               {
                                   Emp.Employee_ID,
                                   Emp.Titile_ID,
                                   Emp.FirstName,
                                   Emp.LastName,
                                   Emp.Department_ID,
                                   Emp.Unit_ID,
                                   Emp.Position_ID,
                                   Emp.Vacation
                               };
                    ReportDataSet.EmpSearchDataTable dt = new ReportDataSet.EmpSearchDataTable();

                    foreach (var data in iEmp.AsEnumerable())
                    {
                        dt.Rows.Add(data.Employee_ID, (from t in db.T_Titles where t.Title_ID == data.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data.FirstName + " " + data.LastName, (from d in db.T_Departments where d.Department_ID == data.Department_ID select d.Departmen).FirstOrDefault(),
                            (from u in db.T_Units where u.Unit_ID == data.Unit_ID select u.Unit).FirstOrDefault(),
                            (from p in db.T_Positions where p.Position_ID == data.Position_ID select p.Position).FirstOrDefault(), data.Vacation + " ວັນ");
                    }

                    dataGrid.ItemsSource = dt.DefaultView;
                    textcount.Text = "";
                }            
                #endregion

                #region 3
                else if (IndexSearch == 3)
                {
                    var iEmp = from Emp in db.T_Employees
                               where Emp.Department_ID == (from d in db.T_Departments where d.Departmen == GlobalVariableClass.TempString select d.Department_ID).FirstOrDefault()
                               select new
                               {
                                   Emp.Employee_ID,
                                   Emp.Titile_ID,
                                   Emp.FirstName,
                                   Emp.LastName,
                                   Emp.Department_ID,
                                   Emp.Unit_ID,
                                   Emp.Position_ID,
                                   Emp.Vacation
                               };
                    ReportDataSet.EmpSearchDataTable dt = new ReportDataSet.EmpSearchDataTable();

                    foreach (var data in iEmp.AsEnumerable())
                    {
                        dt.Rows.Add(data.Employee_ID, (from t in db.T_Titles where t.Title_ID == data.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data.FirstName + " " + data.LastName, (from d in db.T_Departments where d.Department_ID == data.Department_ID select d.Departmen).FirstOrDefault(),
                            (from u in db.T_Units where u.Unit_ID == data.Unit_ID select u.Unit).FirstOrDefault(),
                            (from p in db.T_Positions where p.Position_ID == data.Position_ID select p.Position).FirstOrDefault(), data.Vacation + " ວັນ");
                    }

                    dataGrid.ItemsSource = dt.DefaultView;
                    textcount.Text = "ພະນັກງານຈຳນວນ " + iEmp.Count().ToString() + " ຄົນ";
                }
                #endregion

                #region 4
                else
                {
                    var iEmp = from Emp in db.T_Employees
                               where Emp.Unit_ID == (from d in db.T_Units where d.Unit == GlobalVariableClass.TempString select d.Unit_ID).FirstOrDefault()
                               select new
                               {
                                   Emp.Employee_ID,
                                   Emp.Titile_ID,
                                   Emp.FirstName,
                                   Emp.LastName,
                                   Emp.Department_ID,
                                   Emp.Unit_ID,
                                   Emp.Position_ID,
                                   Emp.Vacation
                               };
                    ReportDataSet.EmpSearchDataTable dt = new ReportDataSet.EmpSearchDataTable();

                    foreach (var data in iEmp.AsEnumerable())
                    {
                        dt.Rows.Add(data.Employee_ID, (from t in db.T_Titles where t.Title_ID == data.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data.FirstName + " " + data.LastName, (from d in db.T_Departments where d.Department_ID == data.Department_ID select d.Departmen).FirstOrDefault(),
                            (from u in db.T_Units where u.Unit_ID == data.Unit_ID select u.Unit).FirstOrDefault(),
                            (from p in db.T_Positions where p.Position_ID == data.Position_ID select p.Position).FirstOrDefault(), data.Vacation + " ວັນ");
                    }

                    dataGrid.ItemsSource = dt.DefaultView;
                    textcount.Text = "ພະນັກງານຈຳນວນ " + iEmp.Count().ToString() + " ຄົນ";
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
