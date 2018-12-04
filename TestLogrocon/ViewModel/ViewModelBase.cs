using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestLogrocon
{
    /// <summary>
    /// Класс реализует взаимодействие различных ViewModel-наследников.
    /// </summary>
   abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Для обновления данных между ViewModel, Model, каждый наслдедник должен реализовывать 
        /// PropertyChangedEventHandler.
        /// </summary>
        public abstract event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Создаём сслыку на окно (View)
        /// </summary>
        public SupportClasses.ChildWindow newWindow = null;

        
        /// <summary>
        ///  
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(ViewModelBase), new PropertyMetadata(""));

        protected virtual void Closed()
        {

        }
        /// <summary>
        /// Закрывает окно
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            bool result = false;
            if (newWindow != null)
            {
                newWindow.Close();
                newWindow = null;
                result = true;
            }
            return result;
        }
        protected void ShowDialog(ViewModelBase viewModel)
        {
            viewModel.newWindow = new SupportClasses.ChildWindow();
            viewModel.newWindow.DataContext = viewModel;
            viewModel.newWindow.Closed += (sender, e) => Closed();
            viewModel.newWindow.ShowDialog();
        }
    }
}
