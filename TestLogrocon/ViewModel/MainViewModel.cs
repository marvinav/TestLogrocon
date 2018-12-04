using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TestLogrocon.ViewModel
{
    class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
      

        RelayCommand removeCommand; // Удаление клиента
        RelayCommand addOrderCommand; // Добавление заказа для выбранного клиента
        RelayCommand removeOrderCommand; // Удаление выбранного заказа
        RelayCommand editOrderCommand; // Редактирование выбранного заказа
        RelayCommand addClientCommand; // Добавление новго клиента

        public MainViewModel(AppContext appContext)
        {            
            // Первичное объявляем клиентов
            ApplicationContext = appContext;
            ApplicationContext.Clients.Load();
            Clients = ApplicationContext.Clients.Local.ToBindingList();
            
        }

        // Ссылка на базу данных
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
        
        Client selectedClient;
        /// <summary>
        /// Выбранный клиент из общего списка Clients
        /// </summary>
        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {   
                selectedClient = value;
                // Подписываем выбранного клиента на событие изменения его свойств
                if (SelectedClient != null)
                { selectedClient.PropertyChanged += SelectedItem_PropertyChanged; }
                
                OnPropertyChanged("SelectedClient");
                // Объявляем список заказов для выбранного клиента
                if (selectedClient != null)
                {
                    Orders = ApplicationContext.Orders.Where(Order => Order.Client_id == value.Client_Id).ToList();
                    IsClientSelected = true;
                }
                else
                {
                    IsClientSelected = false;
                }
            }
        }
        

        IEnumerable<Order> orders;
        public IEnumerable<Order> Orders
        {
            get { return orders; }
            set
            {
                orders = value;
                OnPropertyChanged("Orders");
            }
        }

        IEnumerable<Client> clients;
        public IEnumerable<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged("Clients");
            }
        }
        Boolean isClientSelected;
        /// <summary>
        /// Проверка, выбран ли из списка клиент для отключения/включения кнопоко (IsEnabled)
        /// </summary>
        public Boolean IsClientSelected
        {
            get { return isClientSelected; }
            set
            {
                isClientSelected = value;
                OnPropertyChanged("IsClientSelected");
            }
        }
        // Повторяем процедуры выше и для заказа
        Order selectedOrder;
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                if (selectedOrder != null)
               { selectedOrder.PropertyChanged += SelectedItem_PropertyChanged; }
                OnPropertyChanged("SelectedOrder");
                if (selectedOrder != null)
                {
                    IsOrderSelected = true;
                }
                else
                {
                    IsOrderSelected = false;
                }
            }

        }
        private Boolean isOrderSelected;
        public Boolean IsOrderSelected
        {
            get { return isOrderSelected; }
            set
            {
                isOrderSelected = value;
                OnPropertyChanged("IsOrderSelected");
            }
        }
        /// <summary>
        /// Удаление выбранного клиента из базы
        /// </summary>
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand((o) =>
                  {
                      Client client = o as Client;
                      if (client != null)
                      {
                          ApplicationContext.Clients.Remove(client);
                          ApplicationContext.SaveChanges();
                      }

                  },o=> SelectedClient!=null));
            }
        }
        /// <summary>
        /// Добавление заказа для выбранного клиента
        /// </summary>
        public RelayCommand AddOrderCommand
        {
            get
            {
                return addOrderCommand ??
                  (addOrderCommand = new RelayCommand((o) =>
                  {

                      Client client = o as Client;
                      Order order = new Order();
                      order.Client_id = SelectedClient.Client_Id;
                      ApplicationContext.Orders.Add(order);
                      ApplicationContext.SaveChanges();
                      Orders = ApplicationContext.Orders.Where(Order => Order.Client_id == order.Client_id).ToList();
                      SelectedOrder = order;
                  }, o => SelectedClient != null));
            }
        }
        /// <summary>
        /// Удаление выбранного заказа из базы
        /// </summary>
        public RelayCommand RemoveOrderCommand
        {
            get
            {
                return removeOrderCommand ??
                  (removeOrderCommand = new RelayCommand((o) =>
                  {
                      Order order = o as Order;

                      if (o != null)
                      {

                          ApplicationContext.Orders.Remove(order);
                          ApplicationContext.SaveChanges();
                          Orders = ApplicationContext.Orders.Where(Order => Order.Client_id == order.Client_id).ToList();                        

                      }

                  }, o=>SelectedOrder != null));
            }
        }
        /// <summary>
        /// Создаём DependencyProperty для контролов IsEnabled 
        /// </summary>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.Register("IsEnabled", typeof(bool), typeof(MainViewModel), new PropertyMetadata(true));
        
        /// <summary>
        /// Добавление нового клиента в базу
        /// </summary>
        public RelayCommand AddClientCommand
        {
            get
            {
                return addClientCommand ??
                     (addClientCommand = new RelayCommand((o) =>
                     {
                         ClientViewModel child = new ClientViewModel()
                         {
                             Title = "Добавление нового клиента"

                         };
                         child.ApplicationContext = this.ApplicationContext;
                         ShowDialog(child);
                     }));
            }
        }

        /// <summary>
        /// Вызываем окно для редактирования заказа
        /// </summary>
        public RelayCommand EditOrderCommand
        {
            get
            {
                return editOrderCommand ??
                    (editOrderCommand = new RelayCommand((o) =>
                    {
                        OrderViewModel child = new OrderViewModel()
                        {
                            Title = $"Редактирование заказа номер {SelectedOrder.Number}",
                            SelectedOrder = this.SelectedOrder
                        };
                        ShowDialog(child);
                    }, o=>SelectedOrder != null));
            }
        }
        /// <summary>
        /// Обновляем базу данных при изменении свойств клиента и его заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ApplicationContext.SaveChanges();
        }
        public override event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                ApplicationContext.SaveChanges();
                if (prop == "ClientList")
                {
                    MessageBox.Show("S");
                }
                
            }
            
        }


    }
}
