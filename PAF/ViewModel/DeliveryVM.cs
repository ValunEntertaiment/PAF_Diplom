using PAF.Commands.Base;
using PAF.Data.Entityies;
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
    class DeliveryVM : ViewModelForWindow, IPage
    {
        #region Properties
        public DataTable DataTable { get => _DataTable; set => Set(ref _DataTable, value); }
        DataTable _DataTable;

        public DataTable SubTable { get => _SubTable; set => Set(ref _SubTable, value); }
        DataTable _SubTable;

        public DataRowView SelectedDelivery
        {
            get => _SelectedDelivery;
            set
            {
                Set(ref _SelectedDelivery, value);
                if (SelectedDelivery != null)
                {
                    SubRefresh(SelectedDelivery.Row.ItemArray[0]);
                }
            }
        }
        DataRowView _SelectedDelivery;

        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        /// <summary>Данные нового клиента</summary>
        public Deliveries AddDelivery { get => _AddDelivery; set => Set(ref _AddDelivery, value); }
        Deliveries _AddDelivery = new Deliveries();
        #endregion

        private void Refresh()
        {
            string query = "select d.Id Код," +
                                "s.Name Поставщик, " +
                                "d.date 'Дата поставки', " +
                                "sum(dc.Sum) 'Сумма поставки' " +
                        "from Deliveries d " +
                        "left join DeliveriesCompositions dc on dc.Delivery_Id = d.Id " +
                        "left join Supplies s on s.Id = Supply_Id " +
                        "group by d.Id, s.Name, d.date";
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
                MessageBox.Show(x.Message, "Delivery");
            }
        }

        private void SubRefresh(object id)
        {
            string subQuery =
                        "select dc.Id Код, "+
                                "dc.Price Цена, "+
                                "dc.Amount Количество, "+
                                "dc.Sum Сумма, "+
                                "c.[Name] Товар, "+
		                        "s.[Name] Поставщик "+
                        "from DeliveriesCompositions dc "+
                        "inner join Components c on c.Id = dc.Component_Id "+
                        "inner join Deliveries d on dc.Delivery_id = d.Id "+
                        "inner join Supplies s on s.Id = d.Supply_Id "+
                        $"where d.Id = {(int)id}";
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
                MessageBox.Show(x.Message,"Delivery");
            }
        }


        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }

        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            CanButtonClick = false;
            DeliveryAdd delivery = new DeliveryAdd();
            delivery.ShowDialog();
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

        public DeliveryVM()
        {
            #region Commands
            SaveChangesCommand = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            UpdateCommand = new LambdaCommand(OnUpdateExecuted, CanUpdateExecute);
            #endregion

            Refresh();
        }
    }
}
