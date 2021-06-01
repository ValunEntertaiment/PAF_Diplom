using PAF.Data.Clases;
using PAF.Data.Entityies;
using PAF.ViewModel;
using PAF.ViewModel.BaseVM;
using System;
using System.Windows;
using System.Windows.Input;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClientAdd.xaml
    /// </summary>
    public partial class ClientAdd : Window
    {
        Clients client = new Clients();

        public ClientAdd(IPage page)
        {
            InitializeComponent();
            this.DataContext = page;
            ClientGender.ItemsSource = Enum.GetValues(typeof(Genders));
            ClientGender.SelectedValue = Genders.Муж;
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

        private void ButtonClientAdd(object sender, RoutedEventArgs e)
        {
            client.LastName = ClientLastName.Text;
            client.FirstName = ClientFirstName.Text;
            client.MiddleName = ClientMiddleName.Text;

            if (ClientGender.SelectedValue == null)
                client.Gender = Genders.Муж;
            else
                client.Gender = (Genders)ClientGender.SelectedValue;

            client.Phone = ClientPhone.Text;


            new SQLClient().InsertClient(client);
            
            this.Close();
        }
    }
}

