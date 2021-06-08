using PAF.Data.Clases;
using PAF.Data.Entityies;
using PAF.ViewModel;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

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
            employee.LastName = LastName.Text;
            employee.FirstName = FirstName.Text;
            employee.MiddleName = MiddleName.Text;

            if (Gender.SelectedValue == null)
                employee.Gender = Genders.Муж;
            else
                employee.Gender = (Genders)Gender.SelectedValue;
            int gender = employee.Gender == Genders.Муж ? 0 : 1;

            try
            {
                employee.Salary = Convert.ToDecimal(Salary.Text);
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                    {
                        connection.Open();
                        string q =
                                    "insert into Employees(LastName,FirstName,MiddleName,Gender,Salary) " +
                                    $"values ('{employee.LastName}','{ employee.FirstName}','{ employee.MiddleName}',{gender},{ employee.Salary}) ";
                        SqlCommand command = new SqlCommand(q, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    Close();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "EmployeeAdd");
                }
            }
            catch
            {
                MessageBox.Show("Зарплата введена не корректно");
            }
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
