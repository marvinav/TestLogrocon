using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestLogrocon
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Создаём базу данных приложения
            AppContext appContext = new AppContext();
            //  Создаём VM главного окна
            ViewModel.MainViewModel mainViewModel = new ViewModel.MainViewModel(appContext) {Title = "База данных клиентов" };
            // Связываем данные модели с базой данных
            mainViewModel.ApplicationContext = appContext;
            // Создаём окно
            mainViewModel.newWindow = new SupportClasses.ChildWindow();
            mainViewModel.newWindow.DataContext = mainViewModel;
            // Запускаем
            mainViewModel.newWindow.Show();

        }
    }

}
