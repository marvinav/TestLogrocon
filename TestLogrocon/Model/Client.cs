using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestLogrocon
{
    public class Client : INotifyPropertyChanged, IDataErrorInfo
    {
        private int client_Id;
        private string name;
        private string address;
        private Boolean vip;

        [Key]
        public int Client_Id
        {
            get
            {
                return client_Id;
            }
            set
            {
                client_Id = value;
                OnPropertyChanged("Client_Id");
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        public Boolean VIP
        {
            get
            {
                return vip;
            }
            set
            {
                vip = value;
                OnPropertyChanged("VIP");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop ="")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// Валидация ввода атрибутов клиента
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (Name == null || Name == "")
                        {                            
                            fields.NameCheck = false;
                            error = "Нельзя добавлять безымянного клиента.";
                        }
                        else fields.NameCheck=true; // Вычитаем ошибку у счётчика
                        break;
                    case "Address":
                        if (Address == null || Address == "")
                        {                           
                            fields.AddressCheck = false;
                            error = "У клиента должен быть адресс.";
                        }
                        else fields.AddressCheck = true;
                        break;
                }
                CanSave = (error == String.Empty && fields.Check);
                return error;
            }
        }

        /// <summary>
        /// Проверяем, есть ли во входных данных ошибки
        /// </summary>
        [NotMapped]        
        public bool CanSave
        {
            get { return canSave; }
            set { canSave = value; OnPropertyChanged("CanSave"); }
        }
        private bool canSave;

       
        Fields fields;
        /// <summary>
        /// Валидация данных вода, false - если ошибки
        /// </summary>
        struct Fields
        {
            internal Boolean NameCheck;
            internal Boolean AddressCheck;
            internal Boolean Check
            {
                get
                {
                    if (NameCheck == true && AddressCheck == true)
                    {
                        return true;
                    }
                    else return false;
                }
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
   
}
