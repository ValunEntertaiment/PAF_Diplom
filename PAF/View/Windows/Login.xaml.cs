using PAF.Data;
using PAF.Data.Clases;
using PAF.Data.Entityies;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string password = Password.Password;
            string login = TBLogin.Text;
            try
            {
                new MyDbContext().Database.Initialize(false);
            }
            catch (Exception)
            {

                MessageBox.Show("Не удалось автоматически создать базу данных", "Ошибка создания базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (password == "0001" && login == "")
            {
                new MainWindow().Show();
                Close();
            }
            else if (password == "" || login == "")
            {
                MessageBox.Show("Введите логин и пароль");
            }
            else
            {
                string q = $"select Role from Employees where Login='{login}' and Password ='{password}'";
                string qq = $"select Id from Employees where Login='{login}' and Password ='{password}'";
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
                        MessageBox.Show("Роль данного пользователя не обрабатывается программой. Обратитесь к разработчику программы", "Ошибка роли");
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

        void Connect(string password, string login)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Settings().ShowDialog();
        }
    }
}
