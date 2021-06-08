using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SupplyAdd.xaml
    /// </summary>
    public partial class SupplyAdd : Window
    {
        public SupplyAdd()
        {
            InitializeComponent();
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

        private void AddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    connection.Open();
                    string q =
                                "insert into Supplies([Name], Address) " +
                                $"values ('{Name.Text}','{ Adress.Text}') ";
                    SqlCommand command = new SqlCommand(q, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Client");
            }

        }

        private void CancleClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
