using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestLogrocon.ViewModel
{
    class OrderViewModel : ViewModelBase
    {
        /// <summary>
        /// Команда редактирования заказа
        /// </summary>
        RelayCommand editOrderCommand;
        /// <summary>
        /// Указатель выбранного заказа
        /// </summary>
        Order selectedOrder;
        /// <summary>
        /// Заказ, который отправлен на редактирование
        /// </summary>
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                // Создаём новый заказ после редактирования
                NewOrder = new Order();
                // Передаём редактируемые свойства выбранного заказа в новые заказ
                NewOrder.Description = SelectedOrder.Description;
                OnPropertyChanged("SelectedOrder");
            }
        }

        /// <summary>
        /// Заказ, после редактирования
        /// </summary>
        public Order NewOrder { get; set; }
        /// <summary>
        /// Редактирование выбранного заказа
        /// </summary>
        public RelayCommand EditOrderCommand
        {
            get
            {
                return editOrderCommand ??
                  (editOrderCommand = new RelayCommand((o) =>
                  {
                      // Задаем выбранному заказау значения, отредактированного заказа
                      SelectedOrder.Description = NewOrder.Description;
                      // Закрываем окно
                      Close();
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
