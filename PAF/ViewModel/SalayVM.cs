using PAF.Commands.Base;
using PAF.View.Windows;
using PAF.ViewModel.BaseVM;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class SalayVM : ViewModelForWindow, IPage
    {
        #region Properties
        public DataTable DataTable { get => _DataTable; set => Set(ref _DataTable, value); }
        DataTable _DataTable;

        public DataTable SubTable { get => _SubTable; set => Set(ref _SubTable, value); }
        DataTable _SubTable;

        public DataRowView SelectedItem
        {
            get => _SelectedItem;
            set
            {
                Set(ref _SelectedItem, value);
                if (_SelectedItem != null)
                {
                    SubRefresh(_SelectedItem.Row.ItemArray[0]);
                }
            }
        }
        
        DataRowView _SelectedItem;

        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;
        #endregion

        private void Refresh()
        {
            string query =
"select s.Id Код, " +
    "e.FirstName Сотрудник, " +
    "s.date 'Дата продажи', " +
    "sum(sc.Sum) 'Сумма продажи' " +
"from Sales s " +
"left join SalesCompositions sc on sc.Sale_Id = s.Id " +
"left join Employees e on e.Id = Employee_Id " +
"group by s.Id, e.FirstName, s.date; ";
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
                MessageBox.Show(x.Message, "Salay");
            }
        }

        private void SubRefresh(object id)
        {
            string subQuery =
"select sc.Id Код, "+
    "sc.Price Цена, "+
    "sc.Amount Количество, "+
    "sc.Sum Сумма, "+
    "c.[Name] Товар, "+
    "e.FirstName Поставщик"+
"from SalesCompositions sc"+
"inner join Components c on c.Id = sc.Component_Id"+
"inner join Sales s on sc.Sale_Id = s.Id" +
"inner join Employees e on e.Id = s.Employee_Id" +
$"where s.Id = {(int)id}";
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
                MessageBox.Show(x.Message, "Delivery");
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
            Refresh();
            CanButtonClick = true;

        }
        #endregion

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;
            new SalayAdd().ShowDialog();
            Refresh();
            CanButtonClick = true;

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
            CanButtonClick = false;
            MessageBox.Show("Delete");
            CanButtonClick = true;
        }
        #endregion
        #endregion

        public SalayVM()
        {
            #region Commands
            SaveChangesCommand = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            AddCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            UpdateCommand = new LambdaCommand(OnUpdateExecuted, CanUpdateExecute);
            DeleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            #endregion
        }
    }
}