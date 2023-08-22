using EDLpakse.DialogBox;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace EDLpakse.WPF
{
    /// <summary>
    /// Interaction logic for EmpInputWindow.xaml
    /// </summary>
    public partial class EmpInputWindow : Window
    {
        public EmpInputWindow()
        {
            InitializeComponent();
          
        }
        string picFileName = "";
        int AllRecord = 0;
        int CurrentRecord = 0;
        Bitmap CBMP;
        EDLpakseDataClassesDataContext db = new EDLpakseDataClassesDataContext();

        private void BtnInsertPic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OpenDlg = new OpenFileDialog();
                OpenDlg.Title = "ເລືອກໄຟລຮູບພາບ";
                OpenDlg.Filter = "JPEG (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp";
                OpenDlg.Multiselect = false;
                OpenDlg.FilterIndex = 0;
                if (OpenDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string picFileName = "";
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

        private void BtnDelPic_Click(object sender, RoutedEventArgs e)
        {
            image.Source = new BitmapImage(new Uri("../Image/DefaultImage.jpg", UriKind.Relative));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newEmp = from em in db.T_Employees
                             where em.Employee_ID.ToString() == txtidemp.Text
                             select em;

                if (newEmp.Count() != 1)
                {
                    #region Insert

                    T_Employee emp = new T_Employee();
                    byte[] buffer;
                    var bitmap = image.Source as BitmapSource;
                    var encoder = new PngBitmapEncoder();
                    emp.Employee_ID = Convert.ToInt32(txtidemp.Text);
                    emp.Titile_ID = (from t in db.T_Titles where t.Title == comboBoxTitleName.Text select t.Title_ID).FirstOrDefault();
                    emp.FirstName = txtName.Text;
                    emp.LastName = txtSurname.Text;
                    emp.Culture_ID = (from c in db.T_Cultures where c.Culture == comboBoxCulture.Text select c.Culture_ID).FirstOrDefault();
                    emp.Education_ID = (from ed in db.T_Educations where ed.Education == comboBoxEducation.Text select ed.Education_ID).FirstOrDefault();
                    emp.Country_ID = (from co in db.T_Countries where co.Country == comboBoxFrom.Text select co.Country_ID).FirstOrDefault();
                    emp.Staff_ID = (from s in db.T_Kind_staffs where s.Staff_Kind == comboBoxStatus.Text select s.Staff_ID).FirstOrDefault();
                    emp.StartWork_Date = comboBoxDay.Text +"/"+ comboBoxMonth.Text + "/" + comboBoxYear.Text;
                    emp.Department_ID = (from d in db.T_Departments where d.Departmen == comboBoxDepartment.Text select d.Department_ID).FirstOrDefault();
                    emp.Position_ID = (from p in db.T_Positions where p.Position == comboBoxPosition.Text select p.Position_ID).FirstOrDefault();
                    emp.Unit_ID = (from u in db.T_Units where u.Unit == comboBoxUnit.Text select u.Unit_ID).FirstOrDefault();
                    emp.WorkExperial = txtNote.Text;
                    emp.Vacation = "15";


                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    using (var stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        buffer = stream.ToArray();
                    }
                    emp.Photo = buffer;

                    db.T_Employees.InsertOnSubmit(emp);                  
                    db.SubmitChanges();

                    #endregion
                }

                else
                {
                    #region update

                    byte[] CurrentImage;
                   
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

                    #endregion
                }

                SuccesfullDialog frm2 = new SuccesfullDialog();
                frm2.ShowDialog();

                CleanData();

                var All = (from em in db.T_Employees
                           orderby em.Employee_ID descending
                           select new { ລະຫັດ = em.Employee_ID, ຊື່ແລະນາມສະກຸນ = em.FirstName + " " + em.LastName }).Take(10);

                dataGrid.ItemsSource = All;

                AllRecord = Convert.ToInt32(All.Count());

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            CleanData();
        }
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var All = (from em in db.T_Employees
                                        orderby em.Employee_ID descending
                                        select new { ລະຫັດ = em.Employee_ID, ຊື່ແລະນາມສະກຸນ = em.FirstName + " " + em.LastName }).Take(10);

                dataGrid.ItemsSource = All;

                AllRecord = Convert.ToInt32(All.Count());

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

        private void BtnFirst_Click(object sender, RoutedEventArgs e)
        {
            CurrentRecord = 0;
            ShowCurrentHuman(CurrentRecord);
        }
       
        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            CurrentRecord--;
            if (CurrentRecord <= 0)
            {
                CurrentRecord = 0;
            }
            ShowCurrentHuman(CurrentRecord);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CurrentRecord++;
            if (CurrentRecord >= AllRecord)
            {
                CurrentRecord = AllRecord - 1;
            }
            ShowCurrentHuman(CurrentRecord);
            
        }

        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hs = (from h in db.T_Employees orderby h.Employee_ID descending select h).Take(1);
                CurrentRecord = AllRecord;
                txtidemp.Text = hs.FirstOrDefault().Employee_ID.ToString();
                comboBoxTitleName.SelectedValue = (from t in db.T_Titles where t.Title_ID == hs.FirstOrDefault().Titile_ID select t.Title).FirstOrDefault();
                txtName.Text = hs.FirstOrDefault().FirstName;
                txtSurname.Text = hs.FirstOrDefault().LastName;
                comboBoxCulture.SelectedValue = (from t in db.T_Cultures where t.Culture_ID == hs.FirstOrDefault().Culture_ID select t.Culture).FirstOrDefault();
                comboBoxEducation.SelectedValue = (from t in db.T_Educations where t.Education_ID == hs.FirstOrDefault().Education_ID select t.Education).FirstOrDefault();
                comboBoxFrom.SelectedValue = (from t in db.T_Countries where t.Country_ID == hs.FirstOrDefault().Country_ID select t.Country).FirstOrDefault();
                comboBoxStatus.SelectedValue = (from t in db.T_Kind_staffs where t.Staff_ID == hs.FirstOrDefault().Staff_ID select t.Staff_Kind).FirstOrDefault();
                comboBoxDay.Text = hs.FirstOrDefault().StartWork_Date.Split('/')[0];
                comboBoxMonth.Text = hs.FirstOrDefault().StartWork_Date.Split('/')[1];
                comboBoxYear.Text = hs.FirstOrDefault().StartWork_Date.Split('/')[2];
                comboBoxUnit.SelectedValue = (from t in db.T_Units where t.Unit_ID == hs.FirstOrDefault().Unit_ID select t.Unit).FirstOrDefault();
                comboBoxPosition.SelectedValue = (from t in db.T_Positions where t.Position_ID == hs.FirstOrDefault().Position_ID select t.Position).FirstOrDefault();
                comboBoxDepartment.SelectedValue = (from t in db.T_Departments where t.Department_ID == hs.FirstOrDefault().Department_ID select t.Departmen).FirstOrDefault();
                txtNote.Text = hs.FirstOrDefault().WorkExperial;

                if (hs.FirstOrDefault().Photo != null)
                {

                    byte[] b = (hs.FirstOrDefault().Photo).ToArray();
                    MemoryStream memoryStream = new MemoryStream(b);
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = memoryStream;
                    imageSource.EndInit();
                    image.Source = imageSource;
                }
               
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void Window_Closed(object sender, EventArgs e)
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

        private void CleanData()
        {
            image.Source = new BitmapImage(new Uri("../Image/DefaultImage.jpg", UriKind.Relative));
            txtidemp.Text = "";
            comboBoxTitleName.SelectedIndex = 0;
            txtName.Text = "";
            txtSurname.Text = "";
            comboBoxCulture.SelectedIndex = 0;
            comboBoxEducation.SelectedIndex = 0;
            comboBoxFrom.SelectedIndex = 0;
            comboBoxStatus.SelectedIndex = 0;
            comboBoxDay.SelectedIndex =0;
            comboBoxMonth.SelectedIndex = 0;
            comboBoxYear.SelectedIndex = 0;
            comboBoxDepartment.SelectedIndex = 0;
            comboBoxPosition.SelectedIndex = 0;
            comboBoxUnit.SelectedIndex = 0;
            txtNote.Text = "";            
        }

        private void ShowCurrentHuman(int Current)
        {
            try
            {
                var hs = (from h in db.T_Employees orderby h.Employee_ID select h).Skip(Current).Take(1);
                txtidemp.Text = hs.FirstOrDefault().Employee_ID.ToString();
                comboBoxTitleName.SelectedValue = (from t in db.T_Titles where t.Title_ID == hs.FirstOrDefault().Titile_ID select t.Title).FirstOrDefault();
                txtName.Text = hs.FirstOrDefault().FirstName;
                txtSurname.Text = hs.FirstOrDefault().LastName;
                comboBoxCulture.SelectedValue = (from t in db.T_Cultures where t.Culture_ID == hs.FirstOrDefault().Culture_ID select t.Culture).FirstOrDefault();
                comboBoxEducation.SelectedValue = (from t in db.T_Educations where t.Education_ID == hs.FirstOrDefault().Education_ID select t.Education).FirstOrDefault();
                comboBoxFrom.SelectedValue = (from t in db.T_Countries where t.Country_ID == hs.FirstOrDefault().Country_ID select t.Country).FirstOrDefault();
                comboBoxStatus.SelectedValue = (from t in db.T_Kind_staffs where t.Staff_ID == hs.FirstOrDefault().Staff_ID select t.Staff_Kind).FirstOrDefault();
                comboBoxDay.Text = hs.FirstOrDefault().StartWork_Date.Split('/')[0];
                comboBoxMonth.Text = hs.FirstOrDefault().StartWork_Date.Split('/')[1];
                comboBoxYear.Text = hs.FirstOrDefault().StartWork_Date.Split('/')[2];
                comboBoxUnit.SelectedValue = (from t in db.T_Units where t.Unit_ID == hs.FirstOrDefault().Unit_ID select t.Unit).FirstOrDefault();
                comboBoxPosition.SelectedValue = (from t in db.T_Positions where t.Position_ID == hs.FirstOrDefault().Position_ID select t.Position).FirstOrDefault();
                comboBoxDepartment.SelectedValue = (from t in db.T_Departments where t.Department_ID == hs.FirstOrDefault().Department_ID select t.Departmen).FirstOrDefault();
                txtNote.Text = hs.FirstOrDefault().WorkExperial;

                if (hs.FirstOrDefault().Photo != null)
                {

                    byte[] b = (hs.FirstOrDefault().Photo).ToArray();
                    MemoryStream memoryStream = new MemoryStream(b);
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = memoryStream;
                    imageSource.EndInit();
                    image.Source = imageSource;
                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((string)("ເກິດຂໍ້ຜິດພາດ ເນື່ອງຈາກ " + ex.Message), "ຜົນການທຳງານ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
