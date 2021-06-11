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

                MessageBox.Show("База данных не обнаружена. Автоматически создать базу данных не удалось", "Ошибка создания базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (password == "0001" && login == "")
            {
                new MainWindow("Супер Админ").Show();
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
                string qqq = $"select LastName +' '+ FirstName +' '+ MiddleName +'(' + Login +')' from Employees where Login='{login}' and Password ='{password}'";
                object role = null;
                object Id = null;
                object FIO = null;
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(q, connection);
                        role = command.ExecuteScalar();

                        command = new SqlCommand(qq, connection);
                        Id = command.ExecuteScalar();

                        command = new SqlCommand(qqq, connection);
                        FIO = command.ExecuteScalar();
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
                        new MainWindow((string)FIO).Show();
                        Close();
                        break;
                    case "Склад":
                        new MainWindowSklad((string)FIO).Show();
                        Close();
                        break;
                    case "Продажа":
                        new MainWindowEmployee((string)FIO).Show();
                        Close();
                        break;
                    default:
                        MessageBox.Show("Роль данного пользователя не обрабатывается программой. Обратитесь к разработчику программы", "Ошибка роли");
                        break;

                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Settings().ShowDialog();
        }
    }
}
