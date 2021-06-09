using PAF.Commands.Base;
using PAF.ViewModel.BaseVM;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
//using System.Windows;
using System.Windows.Forms;
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
                        "e.FirstName Поставщик "+
                    "from SalesCompositions sc "+
                    "inner join Components c on c.Id = sc.Component_Id "+
                    "inner join Sales s on sc.Sale_Id = s.Id " +
                    "inner join Employees e on e.Id = s.Employee_Id " +
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

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.csv) | *.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;

                string[] file = File.ReadAllLines(filename);
                string[] row;
                string query;
                bool first = false;
                object DeliveryId = null;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        string q =
                                    "insert into Deliveries(Date, Supply_Id) " +
                                    $@"values (GetDate(),{ SelectedItem.Row.ItemArray[0]}) " +
                                    "select scope_identity()";
                        SqlCommand command = new SqlCommand(q, connection);

                        //SqlDataReader reader = command.ExecuteReader();
                        //if (reader.HasRows)
                        DeliveryId = command.ExecuteScalar();
                        //reader.Close();
                        connection.Close();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                }
                var Id = Convert.ToInt32(DeliveryId);
                foreach (var item in file)
                {
                    if (first)
                    {
                        row = item.Split(';');

                        #region query
                        query =
                            "declare @IdComponent int, " +
                                    "@IdDeliveries int, " +
                                    "@IdType int, " +
                                    "@Component Varchar(255), " +
                                    "@Amount int " +

                            "select @Amount = Amount, @IdComponent = Id " +
                            "From Components " +
                            $@"where[Name] = '{row[0]}' " +

                            //проверяется поставлялся ли этот товар раньше
                            "if (isnull(@Amount, -1) = -1) " +
                                "begin " +//Добавяет полное описание и количество
                                    $@"select @IdType = Id from Types where [Name] = '{row[1]}' " +


                                    "if (isnull(@IdType, 0) = 0) " + //если типа нет то он создает его без указания короткого названия
                                       "begin " +
                                            "insert into Types([Name]) " +
                                            $@"values (convert(nvarchar(MAX),'{row[1]}'))  " +
                                            "set @IdType = scope_identity() " +
                                        "end " +
                                    "insert into Components([Name],/*Price,*/ Amount, Supply_Id, [Type_Id]) " +

                                    $@"values(N'{row[0]}',/*{row[2]},*/ {row[3]}, {SelectedItem.Row.ItemArray[0]}, @IdType) " +

                                    "set @IdComponent = scope_identity() " +



                                "end " +
                            "else " + //Данные о товаре уже есть меняется только количество
                                $@"update Components set Amount = @Amount +{row[3]} " +
                                $@"where[Name] = '{row[0]}' " +
                                "insert into DeliveriesCompositions(Price, Amount, Sum, Component_Id, Delivery_Id) " +

                                    $@"values({row[2]},{row[3]},{row[4]},@IdComponent,{Id}) ";
                        #endregion

                        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                        {
                            try
                            {
                                SqlCommand command = new SqlCommand(query, connection);
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                            catch (Exception x)
                            {
                                MessageBox.Show(x.Message);
                            }
                        }
                    }
                    first = true;
                }
            }
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
        #endregion

        public SalayVM()
        {
            #region Commands
            AddCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            UpdateCommand = new LambdaCommand(OnUpdateExecuted, CanUpdateExecute);
            #endregion

            Refresh();
        }
    }
}