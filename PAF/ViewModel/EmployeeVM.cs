using PAF.Commands.Base;
using System.Windows;
using System.Windows.Input;
using PAF.View.Windows;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Configuration;
using PAF.ViewModel.BaseVM;

namespace PAF.ViewModel
{
    class EmployeeVM : ViewModel, IPage
    {
        #region Properties
        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        public DataRowView SelectedEmployee
        {
            get => _SelectedEmployee;
            set
            {
                Set(ref _SelectedEmployee, value);
                if (SelectedEmployee != null)
                {
                    SubRefresh(SelectedEmployee.Row.ItemArray[0]);
                }
            }
        }
        DataRowView _SelectedEmployee;

        public DataTable DataTable { get => _DataTable; set => Set(ref _DataTable, value); }
        DataTable _DataTable;

        public DataTable Sales { get => SubTable; set => Set(ref SubTable, value); }
        DataTable SubTable;

        public int Width { get => _Width; set => Set(ref _Width, value); }
        int _Width = 800;

        public int Height { get => _Height; set => Set(ref _Height, value); }
        int _Height = 475;
        #endregion

        private void Refresh()
        {
            string query = "SELECT " +
                           "Id код, " +
                           "LastName Фамилия, " +
                           "FirstName Имя, " +
                           "MiddleName Отчество, " +
                           "CASE Gender " +
                               "when 1 then 'Жен' " +
                               "when 0 then 'Муж' " +
                           "END Пол, " +
                           "Salary Зарплата, " +
                           "[Login] Логин, " +
                           "[Password] Пароль, " +
                           "Role Роль " +
                       "FROM Employees";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);
                    DataTable = temp; //добавил temp чтобы срабатывал set у свойства
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void SubRefresh(object id)
        {
            string subQuery =
                        "select " +
                            "SalesCompositions.Id код, " +
                            "SalesCompositions.Price Цена, " +
                            "SalesCompositions.Amount Количество, " +
                            "SalesCompositions.Sum Сумма, " +
                            "Components.Name Товар " +
                        "from SalesCompositions " +
                            "left join Sales on Sales.Id = SalesCompositions.Sale_Id " +
                            "inner join Employees on Sales.Employee_Id = Employees.Id " +
                            "inner join Components on Components.Id = SalesCompositions.Component_Id " +
                        $"where Employees.id = {(int)id} ";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(subQuery, connection);
                    DataTable temp = new DataTable();
                    adapter.Fill(temp);
                    Sales = temp; //добавил temp чтобы срабатывал set у свойства
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void Upload()
        {
            try
            {
                string query = "SELECT " +
                           "Id код, " +
                           "LastName Фамилия, " +
                           "FirstName Имя, " +
                           "MiddleName Отчество, " +
                           "CASE Gender " +
                               "when 'Жен' then 1 " +
                               "when 'Муж' then 0 " +
                           "END Пол, " +
                           "Salary Зарплата, " +
                           "[Login] Логин, " +
                           "[Password] Пароль, " +
                           "Role Роль " +
                       "FROM Employees";
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
                MessageBox.Show(x.Message);
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
            new EmployeeAdd().ShowDialog();
            CanButtonClick = true;
            Refresh();
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
            if (SelectedEmployee != null)
            {
                CanButtonClick = false;
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                    {
                        connection.Open();
                        string q =
                                    "Delete from Employees " +
                                    $"where Id= {SelectedEmployee.Row.ItemArray[0]}";
                        SqlCommand command = new SqlCommand(q, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "EmployeeAdd");
                }
                Refresh();
                CanButtonClick = true;
            }
        }
        #endregion

        #endregion

        public EmployeeVM(ref int Width, ref int Height)
        {
            #region Commands
            SaveChangesCommand = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            AddCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            UpdateCommand = new LambdaCommand(OnUpdateExecuted, CanUpdateExecute);
            DeleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            #endregion

            Refresh();

            this.Width = Width >= 1150 ? Width - 350 : 800;          //1150 минимальная ширина окна. 350-Сумма ширины всех статичных элементов. 800-Минимальная ширина страницы
            this.Height = Height >= 600 ? Height - 80 : 520;   //600 минимальная высота окна. 125-Сумма высоты всех статичных элементов. 475-Минимальная высота страницы
        }

        public EmployeeVM()
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