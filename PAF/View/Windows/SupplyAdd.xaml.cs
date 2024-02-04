using PAF.Data.Entityies;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SupplyAdd.xaml
    /// </summary>
    public partial class SupplyAdd : Window
    {
        Supplies supply = new Supplies();
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
            bool ok = true;

            if (TBName.Text == "")
            {
                ok = false;
                NameText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                NameText.Foreground = new SolidColorBrush(Colors.Gray);
                supply.Name = TBName.Text;
            }

            if (Adress.Text == "")
            {
                ok = false;
                AdressText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                AdressText.Foreground = new SolidColorBrush(Colors.Gray);
                supply.Address = Adress.Text;
            }

            if(ok)
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                    {
                        connection.Open();
                        string q =
                                    "insert into Supplies([Name], Address) " +
                                    $"values ('{TBName.Text}','{ Adress.Text}') ";
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
