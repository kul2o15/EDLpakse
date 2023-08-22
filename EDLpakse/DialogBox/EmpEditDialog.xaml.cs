using System;
using System.Windows;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace EDLpakse.DialogBox
{
    /// <summary>
    /// Interaction logic for EmpEditDialog.xaml
    /// </summary>
    public partial class EmpEditDialog : Window
    {
        public EmpEditDialog()
        {
            InitializeComponent();
        }

        string picFileName = "";
        Bitmap CBMP;
        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();      

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] CurrentImage;
                var newEmp = from em in db.T_Employees
                             where em.Employee_ID.ToString() == GlobalVariableClass.TempString2
                             select em;

                foreach (T_Employee emp in newEmp)
                {

                    emp.Titile_ID = (from t in db.T_Titles where t.Title == comboBoxTitleName.Text select t.Title_ID).FirstOrDefault();
                    emp.FirstName = txtName.Text;
                    emp.LastName = txtSurname.Text;
                    emp.Culture_ID = (from c in db.T_Cultures where c.Culture == comboBoxCulture.Text select c.Culture_ID).FirstOrDefault();
                    emp.Education_ID = (from ed in db.T_Educations where ed.Education == comboBoxEducation.Text select ed.Education_ID).FirstOrDefault();
                    emp.Country_ID = (from co in db.T_Countries where co.Country == comboBoxFrom.Text select co.Country_ID).FirstOrDefault();
                    emp.Staff_ID = (from s in db.T_Kind_staffs where s.Staff_Kind == comboBoxStatus.Text select s.Staff_ID).FirstOrDefault();
                    emp.StartWork_Date = comboBoxDay.Text + "/" + comboBoxMonth.Text + "/" + comboBoxYear.Text;
                    emp.Department_ID = (from d in db.T_Departments where d.Departmen == comboBoxDepartment.Text select d.Department_ID).FirstOrDefault();
                    emp.Position_ID = (from p in db.T_Positions where p.Position == comboBoxPosition.Text select p.Position_ID).FirstOrDefault();
                    emp.Unit_ID = (from u in db.T_Units where u.Unit == comboBoxUnit.Text select u.Unit_ID).FirstOrDefault();
                    emp.WorkExperial = txtNote.Text;
                    emp.EndWork_Date = comboBoxEDay.Text + "/" + comboBoxEMonth.Text + "/" + comboBoxEYear.Text;

                    if (image.Source != null)
                    {

                        if (picFileName != "")
                        {
                            var fs = new FileStream(picFileName, FileMode.Open, FileAccess.Read);
                            CurrentImage = new byte[Convert.ToInt32(fs.Length) + 1];
                            fs.Read(CurrentImage, 0, Convert.ToInt32(fs.Length - 1));
                            fs.Close();
                            emp.Photo = CurrentImage;
                        }

                    }

                }

                db.SubmitChanges();           
                this.DialogResult = true;
                
            }


            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnInsertPic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OpenDlg = new OpenFileDialog();
                OpenDlg.Title = "ເລືອກໄຟລຮູບພາບ";
                OpenDlg.Filter = "JPEG (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp";
                OpenDlg.FileName = "";
                picFileName = "";
                OpenDlg.Multiselect = false;
                OpenDlg.FilterIndex = 0;
                if (OpenDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    picFileName = OpenDlg.FileName;
                    CBMP = new Bitmap(picFileName);
                    if (CBMP != null)
                    {
                        if (CBMP.Height < 185 || CBMP.Width < 135)
                        {
                            NotFoundDialog frm = new NotFoundDialog();
                            frm.label1.Text = " ໄຟລຮູບພາບທີ່ໃຊ້ ຕ້ອງມີຂະໜາດໃຫຍ່ກວ່າ 135 x 185 pixel ";
                            frm.ShowDialog();

                            return;
                        }
                        image.Source = new BitmapImage(new Uri(OpenDlg.FileName));
                    }


                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ໄຟລຮູບພາບທີ່ເລືອກ ບໍ່ສາມາດໃຊ້ງານໄດ້ ເນື່ອງຈາກ" + ex.Message, "ຂໍ້ຜິດພາດ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                           
                comboBoxTitleName.ItemsSource = (from t in db.T_Titles
                                                 select t.Title).ToList();

                comboBoxCulture.ItemsSource = (from c in db.T_Cultures
                                               select c.Culture).ToList();

                comboBoxEducation.ItemsSource = (from ed in db.T_Educations
                                                 select ed.Education).ToList();

                comboBoxFrom.ItemsSource = (from fr in db.T_Countries
                                            select fr.Country).ToList();

                comboBoxStatus.ItemsSource = (from s in db.T_Kind_staffs
                                              select s.Staff_Kind).ToList();

                comboBoxDepartment.ItemsSource = (from d in db.T_Departments
                                                  select d.Departmen).ToList();

                comboBoxPosition.ItemsSource = (from p in db.T_Positions
                                                select p.Position).ToList();

                comboBoxUnit.ItemsSource = (from u in db.T_Units
                                            select u.Unit).ToList();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
