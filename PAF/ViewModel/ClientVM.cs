using PAF.Commands.Base;
using PAF.View.Windows;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System;
using PAF.ViewModel.BaseVM;
using System.Configuration;

namespace PAF.ViewModel
{
    public class ClientVM : ViewModelForWindow, IPage
    {
        #region Properties
        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        public DataRowView SelectedClient
        {
            get => _SelectedClient;
            set
            {
                Set(ref _SelectedClient, value);
                if (SelectedClient != null)
                {
                    SubRefresh(SelectedClient.Row.ItemArray[0]);
                }
            }
        }
        DataRowView _SelectedClient;

        public DataTable DataTable { get => _DataTable; set => Set(ref _DataTable, value); }
        DataTable _DataTable;

        public DataTable SubTable { get => _SubTable; set => Set(ref _SubTable, value); }
        DataTable _SubTable;

        //public int Width { get => _Width; set => Set(ref _Width, value); }
        //int _Width = 800;

        //public int Height { get => _Height; set => Set(ref _Height, value); }
        //int _Height = 475;
        #endregion

        public void Refresh()
        {
            string query = "SELECT " +
                           "Id Код, " +
                           "LastName Фамилия, " +
                           "FirstName Имя, " +
                           "MiddleName Отчество, " +
                           "CASE Gender " +
                               "when 1 then 'Жен' " +
                               "when 0 then 'Муж' " +
                           "END Пол, " +
                           "Phone Телефон " +
                       "FROM Clients";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);
                    DataTable = temp; //добавил temp чтобы срабатывал set у свойства
                    DataTable.Columns[0].ReadOnly = true;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Ошибка в таблице Клиенты",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        public void Refresh(string search)
        {
            string query = "SELECT " +
                           "Id Код, " +
                           "LastName Фамилия, " +
                           "FirstName Имя, " +
                           "MiddleName Отчество, " +
                           "CASE Gender " +
                               "when 1 then 'Жен' " +
                               "when 0 then 'Муж' " +
                           "END Пол, " +
                           "Phone Телефон " +
                       "FROM Clients " +
                       $"where convert(varchar,id) +' '+ convert(varchar,LastName) +' '+ convert(varchar,FirstName) +' '+ convert(varchar,Middlename) +' '+ CASE Gender when 1 then 'Жен' when 0 then 'Муж' END +' '+ convert(varchar,Phone) LIKE '%{search}%'";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);
                    DataTable = temp; //добавил temp чтобы срабатывал set у свойства
                    DataTable.Columns[0].ReadOnly = true;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Ошибка в таблице Клиенты", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SubRefresh(object id)
        {
              string subQuery = "select " +
                                    "SalesCompositions.Id Код, " +
                                    "SalesCompositions.Price Цена, " +
                                    "SalesCompositions.Amount Количество, " +
                                    "SalesCompositions.Sum Сумма, " +
                                    "Components.Name Товар " +
                                "from SalesCompositions " +
                                    "left join Sales on Sales.Id = SalesCompositions.Sale_Id " +
                                    "inner join Clients on Sales.Client_Id = Clients.Id " +
                                    "inner join Components on Components.Id = SalesCompositions.Component_Id " +
                                $"where Clients.id = {(int)id}";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(subQuery, connection);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);
                    SubTable = temp; //добавил temp чтобы срабатывал set у свойства
                }

            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message,"Ошибка в таблице Продажи", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Upload()
        {
            try
            {
                //string query = "SELECT " +
                //               "Id Код, " +
                //               "LastName Фамилия, " +
                //               "FirstName Имя, " +
                //               "MiddleName Отчество, " +
                //               "CASE Gender " +
                //                   "when 'Жен' then 1 " +
                //                   "when 'Муж' then 0 " +
                //               "END Пол, " +
                //               "Phone Телефон  from Clients";

                string query = "SELECT " +
                           "Id Код, " +
                           "LastName Фамилия, " +
                           "FirstName Имя, " +
                           "MiddleName Отчество, " +
                           "CASE Gender " +
                               "when 1 then 'Жен' " +
                               "when 0 then 'Муж' " +
                           "END Пол, " +
                           "Phone Телефон " +
                       "FROM Clients";

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand = new SqlCommand(query, connection);
                    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                    adapter.UpdateCommand = builder.GetUpdateCommand();
                    adapter.Update(DataTable);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message,"Ошибка обновления данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }

        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            CanButtonClick = false;
            Upload();
            CanButtonClick = true;
        }
        #endregion

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;
            ClientAdd clientAdd = new ClientAdd(new ClientVM());
            clientAdd.ShowDialog();
            CanButtonClick = true;
            OnUpdateExecuted(null);
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            CanButtonClick = false;
            Refresh();
            CanButtonClick = true;
        }
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand { get; set; }
        private bool CanDeleteExecute(object p) => CanButtonClick;
        private void OnDeleteExecuted(object p)
        {
            if (SelectedClient != null)
            {
                CanButtonClick = false;
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                    {
                        connection.Open();
                        string q =
                                    "Delete from Clients " +
                                    $"where Id= {SelectedClient.Row.ItemArray[0]}";
                        SqlCommand command = new SqlCommand(q, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Ошибка выбранного клиента", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                Refresh();
                CanButtonClick = true;
            }
        }
        #endregion

        #endregion

        public ClientVM(ref int Width, ref int Height)
        {
            #region Commands
            SaveChangesCommand = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            AddCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            UpdateCommand = new LambdaCommand(OnUpdateExecuted, CanUpdateExecute);
            DeleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            #endregion

            Refresh();
            
            //this.Width = Width >=1150?Width - 350:800;          //1150 минимальная ширина окна. 350-Сумма ширины всех статичных элементов. 800-Минимальная ширина страницы
            //this.Height = Height >= 600 ? Height - 80 : 520;   //600 минимальная высота окна. 125-Сумма высоты всех статичных элементов. 475-Минимальная высота страницы
        }

        public ClientVM()
        {
            #region Commands
            SaveChangesCommand = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            AddCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            UpdateCommand = new LambdaCommand(OnUpdateExecuted, CanUpdateExecute);
            DeleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            #endregion

            Refresh();
        }
    }
}