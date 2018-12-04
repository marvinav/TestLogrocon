using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestLogrocon.ViewModel
{
    class ClientViewModel : ViewModelBase
    {
        // Команда добавления клиента
        RelayCommand addCommand;
  
        /// <summary>
        /// Локальное хранилище базы данных
        /// </summary>
        AppContext applicationContext;
        public AppContext ApplicationContext
        {
            get { return applicationContext; }
            set
            {
                applicationContext = value;
                OnPropertyChanged("ApplicationContext");
            }
        }
        Client newClient;
        /// <summary>
        /// Создаём нового клиента
        /// </summary>
        public Client NewClient
        {
            get
            {
                if (newClient == null) newClient = new Client();
                return newClient;
            }
            set
            {
                newClient = value;
                OnPropertyChanged("NewClient");
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      Client client = o as Client;
                      if (client.Name != null && client.Address != null)
                      {
                            ApplicationContext.Clients.Add(client);
                            ApplicationContext.SaveChanges();
                            NewClient = null;
                            Close();
                      }
                  }));
            }
        }
        public override event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

    }
}
