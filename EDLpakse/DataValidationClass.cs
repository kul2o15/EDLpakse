using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDLpakse
{
    public class DataValidationClass : ViewModelBaseClass, IDataErrorInfo
    {
        public DataValidationClass()
        {

        }

        private string idemp;
        public string IdEmp
        {
            get { return idemp; }
            set { idemp = value; RaisePropertyChanged("IdEmp"); }
        }

        private string nameemp;
        public string NameEmp
        {
            get { return nameemp; }
            set { nameemp = value; RaisePropertyChanged("NameEmp"); }
        }

        private string surnameemp;
        public string SurnameEmp
        {
            get { return surnameemp; }
            set { surnameemp = value; RaisePropertyChanged("SurnameEmp"); }
        }

        private string nameAc;
        public string NameAc
        {
            get { return nameAc; }
            set { nameAc = value; RaisePropertyChanged("NameAc"); }
        }

        public string this[string columnName]
        {
            get
            {
                error = string.Empty;
                if (columnName == "IdEmp" && string.IsNullOrWhiteSpace(IdEmp))
                {
                    error = "ວ່າງເປົ່າ";
                }
                else if (columnName == "NameEmp" && string.IsNullOrWhiteSpace(NameEmp))
                {
                    error = "ວ່າງເປົ່າ";
                }
                else if (columnName == "SurnameEmp" && string.IsNullOrWhiteSpace(SurnameEmp))
                {
                    error = "ວ່າງເປົ່າ";
                }
                else if (columnName == "NameAc" && string.IsNullOrWhiteSpace(NameAc))
                {
                    error = "ວ່າງເປົ່າ";
                }


                RaisePropertyChanged("IsValidSave");
                RaisePropertyChanged("IsValidSave2");
                RaisePropertyChanged("IsValidSave3");

                return error;
            }
        }

        private string error = string.Empty;
        public string Error
        {
            get { return error; }
        }


        public bool IsValidSave
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NameEmp) && !string.IsNullOrWhiteSpace(IdEmp) && !string.IsNullOrWhiteSpace(SurnameEmp);
            }
        }

        public bool IsValidSave2
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NameEmp) && !string.IsNullOrWhiteSpace(SurnameEmp);
            }
        }

        public bool IsValidSave3
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NameAc);
            }
        }

    }
}
