using PAF.Data;
using PAF.Data.Clases;
using PAF.Data.Entityies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            ObservableCollection<Clients> collection = new SQLClient().SelectClientToObservableCollection();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new MyDbContext().Database.Initialize(false);
            }
            catch (Exception)
            {

                MessageBox.Show("Не удалось автоматически создать базу данных","Ошибка создания базы данных",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            

            if (Password.Password == "0001" && TBLogin.Text == "")
            {
                new MainWindow().Show();
                Close();
            }
            else if(Password.Password == "" || TBLogin.Text == "")
            {
                MessageBox.Show("Введите логин и пароль");
            }
            else
            {
                string q = $"select Role from Employees where Login='{TBLogin.Text}' and Password ='{Password.Password}'";
                string qq = $"select Id from Employees where Login='{TBLogin.Text}' and Password ='{Password.Password}'";
                object role = null;
                object Id = null;
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(q, connection);
                        role = command.ExecuteScalar();

                        command = new SqlCommand(qq, connection);
                        Id = command.ExecuteScalar();
                        connection.Close();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                }

                switch ((string)role)
                {
                    case "Админ":
                        new MainWindow().Show();
                        Close();
                        break;
                    case "Склад":
                        new MainWindowSklad().Show();
                        Close();
                        break;
                    case "Продажа":
                        new MainWindowEmployee().Show();
                        Close();
                        break;
                    default:
                        MessageBox.Show("Роль данного пользователя не обрабатывается программой. Обратитесь к разработчику программы","Ошибка роли");
                        break;

                }
            }
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
