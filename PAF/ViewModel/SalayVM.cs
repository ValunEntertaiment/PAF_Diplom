using PAF.Commands.Base;
using PAF.ViewModel.BaseVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
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

        public void Refresh()
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
                    DataTable.Columns[0].ReadOnly = true;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Salay");
            }
        }

        public void Refresh(string search)
        {
            string query =
                        "select s.Id Код, " +
                            "e.FirstName Сотрудник, " +
                            "s.date 'Дата продажи', " +
                            "sum(sc.Sum) 'Сумма продажи' " +
                        "from Sales s " +
                        "left join SalesCompositions sc on sc.Sale_Id = s.Id " +
                        "left join Employees e on e.Id = Employee_Id " +
                        "group by s.Id, e.FirstName, s.date " +
                        $"having convert(varchar(max),s.Id) + ' ' + convert(varchar(max),e.FirstName) + ' ' + convert(varchar(max),s.date) + ' ' + convert(varchar(max),sum(sc.Sum)) like '%{search}%' ";
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
                        "sp.[Name] Поставщик "+
                    "from SalesCompositions sc "+
                    "inner join Components c on c.Id = sc.Component_Id "+
                    "inner join Sales s on sc.Sale_Id = s.Id " +
                    "inner join Supplies sp on sp.Id = c.Supply_Id " +
                   // "inner join Employees e on e.Id = s.Employee_Id " +
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

                List<string[]> errors = new List<string[]>();
                List<string> file = new List<string>(File.ReadAllLines(filename));
                string columns = file[0];
                file.Remove(file[0]);
                string[] row;
                string query;
                int number = -1;
                int tempNumber = number;
                object SalesId = null;
                int Id = 1;
                object qwe = null;

                foreach (var item in file)
                {
                    row = item.Split(';');
                    number = Convert.ToInt32(row[0]);
                                                        
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                    {
                        try
                        {
                            connection.Open();
                            string q =
                                    $"if ({row[2]} <= (select Amount from Components where id = {row[3]})) " +
                                        "select 0 " +
                                    "else " +
                                        "select 1";
                            SqlCommand command = new SqlCommand(q, connection);

                            qwe = command.ExecuteScalar();
                            
                            connection.Close();
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Импорт продаж - Создание продажи", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }


                    if (Convert.ToInt32(qwe) == 0)
                    {
                        if (number != tempNumber) //если не равно значит началась новая продажа
                        {
                            tempNumber = number;
                            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                            {
                                try
                                {
                                    connection.Open();
                                    string q =
                                            "insert into Sales(Date, Employee_Id, Client_Id) " +

                                                    $"values(Getdate(),{row[4]},{row[5]}) " +
                                            "select scope_identity() ";
                                    SqlCommand command = new SqlCommand(q, connection);

                                    SalesId = command.ExecuteScalar();
                                    Id = Convert.ToInt32(SalesId);

                                    connection.Close();
                                }
                                catch (Exception x)
                                {
                                    MessageBox.Show(x.Message, "Импорт продаж - Создание продажи", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                        #region query
                        query =

                                "insert into SalesCompositions(Price, Amount, Sum, Component_Id, Sale_Id) " +
                                $"values({row[1]}, {row[2]}, {row[1]} * {row[2]}, {row[3]}, {Id}) " +

                                "update Components " +
                                $"set Amount = Amount - {row[2]} " +
                                $"where Id = {row[3]} ";
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
                                MessageBox.Show(x.Message, "Импорт продаж - Создание состава продажи", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        errors.Add(row);
                    }
                    
                }
                if (errors.Count != 0)
                {
                    MessageBox.Show("Некоторых товаров недостаточно для оформления продаж выберите расположение файла со списокм неоформленных продаж", "Недостаточно для оформления продаж",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    saveFileDialog.Filter = "Text files(*.csv) | *.csv"; //формат файла

                    DateTime dateTime = DateTime.Now; //название формируется по текущей дате

                    saveFileDialog.FileName = "Не достаточно товаров " + dateTime.ToString("dd MM yyyy"); //название файла
                    Encoding.GetEncoding("UTF-8");
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string path = saveFileDialog.FileName;
                        StreamWriter file1 = new StreamWriter(path, false, Encoding.UTF8);
                        file1.WriteLine(columns);
                        foreach (var item in errors)
                        {
                            foreach (var item1 in item)
                            {
                                file1.Write(item1 + ";");
                            }
                            file1.WriteLine();
                        }
                        file1.Close();
                    }
                }
            }
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