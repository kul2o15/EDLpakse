using EDLpakse.DialogBox;
using EDLpakse.WPF;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Data;
using Microsoft.Reporting.WinForms;

namespace EDLpakse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();

        private void Window_Closed(object sender, EventArgs e)
        {
            LoginWindow frm = new LoginWindow();
            frm.Show();
            this.Hide();
        }

        private void NewEmpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EmpInputWindow frm = new EmpInputWindow();
            frm.Show();
            this.Hide();
        }

        private void SearchEmpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EmpDelWindow frm = new EmpDelWindow();
            frm.Show();
            this.Hide();
        }
      
        private void ActivityMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmpSearchEditDialog frm = new EmpSearchEditDialog();
                frm.label1.Text = "ເລືອກກິດຈະກຳທີ່ຕ້ອງກາານ";
                frm.comboBoxTemp.ItemsSource = (from t in db.T_Activities
                                                select t.Activity).ToList();
                frm.ShowDialog();

                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {
                    
                    ActivityWindow frm2 = new ActivityWindow();
                    frm2.Show();
                    frm2.texttitle2.Text = "ກິດຈະກຳ "+ GlobalVariableClass.TempString;
                    GlobalVariableClass.TempString3 = GlobalVariableClass.TempString;
                    this.Hide();
                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void VacatonMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EDLpakseDataClassesDataContext db2 = new EDLpakseDataClassesDataContext();
                EmpSearchEditDialog2 frm = new EmpSearchEditDialog2();
                frm.label1.Text = "ປ້ອນລະຫັດ ຫຼື ຊື່ ພະນັກງານທີ່ຕ້ອງການ";
                frm.ShowDialog();

                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {
                   
                    var iEmp = from Emp in db2.T_Employees
                               where Emp.Employee_ID.ToString() == GlobalVariableClass.TempString || Emp.FirstName == GlobalVariableClass.TempString
                               select Emp;

                    VacationDialog frm2 = new VacationDialog();

                    frm2.txtidemp.Text = iEmp.FirstOrDefault().Employee_ID.ToString();
                    frm2.txtName.Text = (from t in db.T_Titles where t.Title_ID == iEmp.FirstOrDefault().Titile_ID select t.Title).FirstOrDefault() + " " +
                                        iEmp.FirstOrDefault().FirstName + " " + iEmp.FirstOrDefault().LastName;
                    frm2.txtposition.Text = (from t in db.T_Positions where t.Position_ID == iEmp.FirstOrDefault().Position_ID select t.Position).FirstOrDefault();
                    frm2.txtunit.Text = (from t in db.T_Units where t.Unit_ID == iEmp.FirstOrDefault().Unit_ID select t.Unit).FirstOrDefault();
                    frm2.txtdepartment.Text = (from t in db.T_Departments where t.Department_ID == iEmp.FirstOrDefault().Department_ID select t.Departmen).FirstOrDefault();
                    frm2.txtstatus.Text = (from t in db.T_Kind_staffs where t.Staff_ID == iEmp.FirstOrDefault().Staff_ID select t.Staff_Kind).FirstOrDefault();

                    if (iEmp.FirstOrDefault().Photo != null)
                    {
                        byte[] b = (iEmp.FirstOrDefault().Photo).ToArray();
                        MemoryStream memoryStream = new MemoryStream(b);
                        var imageSource = new BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = memoryStream;
                        imageSource.EndInit();
                        frm2.image.Source = imageSource;
                    }

                    if (iEmp.FirstOrDefault().Vacation != "0")
                    {
                        for (int i = 1; i <= Convert.ToInt32(iEmp.FirstOrDefault().Vacation); i++)
                        {
                            frm2.comboBoxVacDay.Items.Add(i);
                        }
                    }
                                    
                    frm2.ShowDialog();

                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void EmpRpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmpSearchEditDialog frm = new EmpSearchEditDialog();
                frm.label1.Text = "ເລືອກກິດຈະກຳທີ່ຕ້ອງກາານ";
                frm.comboBoxTemp.ItemsSource = (from t in db.T_Activities
                                                select t.Activity).ToList();
                frm.ShowDialog();

                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {

                    var Iemp = from pa2 in db.T_Participates
                               join Emp2 in db.T_Employees on pa2.Employee_ID equals Emp2.Employee_ID into newpa
                               from npa in newpa.DefaultIfEmpty()
                               where pa2.Activity_ID == (from ac in db.T_Activities where ac.Activity == GlobalVariableClass.TempString select ac.Activity_ID).FirstOrDefault()
                               select new
                               {
                                   pa2.Employee_ID,
                                   npa.Titile_ID,
                                   npa.FirstName,
                                   npa.LastName,
                                   npa.Department_ID,
                                   npa.Position_ID,
                                   npa.Unit_ID                 
                               };

                    ReportDialog frm2 = new ReportDialog();
                    ReportDataSet.ActivittReportDataTable table = new ReportDataSet.ActivittReportDataTable();
                    DataTable dt = table;

                    foreach (var data2 in Iemp.AsEnumerable())
                    {
                        dt.Rows.Add(data2.Employee_ID, (from t in db.T_Titles where t.Title_ID == data2.Titile_ID select t.Title).FirstOrDefault() + " "
                            + data2.FirstName + " " + data2.LastName, (from t in db.T_Departments where t.Department_ID == data2.Department_ID select t.Departmen).FirstOrDefault(),
                            (from t in db.T_Units where t.Unit_ID == data2.Unit_ID select t.Unit).FirstOrDefault(),
                            (from t in db.T_Positions where t.Position_ID == data2.Position_ID select t.Position).FirstOrDefault());
                    }

                    frm2.reportViewer.LocalReport.ReportEmbeddedResource = "EDLpakse.Report.ActivityReport.rdlc";
                    frm2.reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                    frm2.reportViewer.LocalReport.SetParameters(new ReportParameter("ReportParameter1", GlobalVariableClass.TempString));
                    frm2.reportViewer.RefreshReport();
                    frm2.ShowDialog();

                }

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ChangePassMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChangePassDialog frm = new ChangePassDialog();
            frm.ShowDialog();
        }

        private void NewUserMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddUserDialog frm = new AddUserDialog();
            frm.ShowDialog();
        }

        private void DelUserMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DelUserDialog frm = new DelUserDialog();
            frm.ShowDialog();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog frm = new AboutDialog();
            frm.ShowDialog();
        }

        private void InsertActivityMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InsertActivityDialog frm = new InsertActivityDialog();
            frm.ShowDialog();        
        }

        private void ResetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmpSearchEditDialog2 frm = new EmpSearchEditDialog2();
                frm.label1.Text = "ຢືນຢັນດ້ວຍລະຫັດຜ່ານຂອງທ່ານ";
                frm.ShowDialog();

                if (frm.DialogResult == true && GlobalVariableClass.TempString != "")
                {
                    var Iuser = from h in db.T_Auths
                                where h.User_ID == GlobalVariableClass.UserAuth && h.User_Passward == GlobalVariableClass.TempString
                                select h;

                    if (Iuser.Count() == 0)
                    {
                        NotFoundDialog frm2 = new NotFoundDialog();
                        frm2.label1.Text = " ລະຫັດບໍ່ຖືກຕ້ອງ ";
                        frm2.ShowDialog();
                
                    }
                    else if (Iuser.Count() == 1)
                    {

                        var newEmp = from em in db.T_Employees
                                     select em;

                        foreach (T_Employee emp in newEmp)
                        {
                            emp.Vacation = "15";
                        }

                        db.SubmitChanges();

                    }

                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
