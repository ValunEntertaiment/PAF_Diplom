using PAF.Data.Clases;
using PAF.Data.Entityies;
using PAF.ViewModel;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для EmployeeAdd.xaml
    /// </summary>
    public partial class EmployeeAdd : Window
    {
        Employees employee = new Employees();
        public EmployeeAdd()
        {
            InitializeComponent();
            this.DataContext = new EmployeeVM();
            Gender.ItemsSource = Enum.GetValues(typeof(Genders));
            Gender.SelectedValue = Genders.Муж;
            Role.ItemsSource = Enum.GetValues(typeof(Roles));
            Role.SelectedValue = Roles.Консультант;
        }

        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }

        private void ButtonEmployeeAdd(object sender, RoutedEventArgs e)
        {
            bool ok = true;

            employee.LastName = LastName.Text;
            employee.FirstName = FirstName.Text;
            employee.MiddleName = MiddleName.Text;

            if (Gender.SelectedValue == null)
                employee.Gender = Genders.Муж;
            else
                employee.Gender = (Genders)Gender.SelectedValue;
            int gender = employee.Gender == Genders.Муж ? 0 : 1;
            
            if(Login.Text == null || Login.Text == "")
            {
                LoginText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
                employee.Login = Login.Text;


            if (Password.Text == null || Password.Text == "")
                MessageBox.Show("Введите пароль", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                employee.Password = Password.Text;



            if (Role.SelectedValue != null)
                switch ((Roles)Role.SelectedValue)
                {
                    case Roles.Консультант:
                        employee.Role = "Консультант";
                        break;
                    case Roles.Кладовщик:
                        employee.Role = "Кладовщик";
                        break;
                    case Roles.Администратор:
                        employee.Role = "Администратор";
                        break;
                }
            else
                MessageBox.Show("Роль не введена", "Роль не введена", MessageBoxButton.OK,MessageBoxImage.Warning);

            decimal temp;
            if (decimal.TryParse(Salary.Text, out temp))
                employee.Salary = temp;
            else
                MessageBox.Show("Зарплата введена не корректно","",MessageBoxButton.OK,MessageBoxImage.Warning);

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
            //    {
            //        connection.Open();
            //        string q =
            //                    "insert into Employees(LastName,FirstName,MiddleName,Gender,Salary,login,password,role) " +
            //                    $"values ('{employee.LastName}','{ employee.FirstName}','{ employee.MiddleName}',{gender},{ employee.Salary},{employee.Login},{employee.Password},{employee.Role}) ";
            //        SqlCommand command = new SqlCommand(q, connection);
            //        command.ExecuteNonQuery();
            //        connection.Close();
            //    }
            //    Close();
            //}
            //catch (Exception x)
            //{
            //    MessageBox.Show(x.Message, "EmployeeAdd");
            //}
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
