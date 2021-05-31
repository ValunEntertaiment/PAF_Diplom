using PAF.Commands.Base;
using System.Configuration;
using PAF.Data.Classes;
using PAF.Data.Clases;
using PAF.View.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System;
using PAF.Data.Entityies;

namespace PAF.ViewModel
{
    public class ClientVM : ViewModelForWindow
    {
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

        


        SqlDataAdapter adapter;
        DataTable DataTable;
        DataTable SubTable;
        string connectionString = "Data Source=C-PROG4;Initial Catalog=PAF;Integrated Security=True";
            //ConfigurationManager.ConnectionStrings[0].ConnectionString;

        string query = "SELECT "+
                           "Id код, "+
                           "LastName Фамилия, "+
	                       "FirstName Имя, "+
	                       "MiddleName Отчество, "+
	                       "CASE Gender "+
                               "when 1 then 'Жен' "+
		                       "when 0 then 'Муж' "+
	                       "END Пол, "+
                           "Phone Телефон "+
                       "FROM "+
                       "Clients ";

        string subQuery = "select SalesCompositions.* "+
                          "from SalesCompositions "+
                          "left join Sales on Sales.Id = SalesCompositions.Sale_Id "+
                          "inner join Clients on Sales.Client_Id = Clients.Id ";

        public DataTable Clients { get => DataTable; set => Set(ref DataTable, value); }

        public DataTable Sales { get => SubTable; set => Set(ref SubTable, value); }

        private void Refresh()
        {
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    adapter = new SqlDataAdapter(query, connection);
                    Clients = new DataTable();
                    adapter.Fill(Clients);
                }

            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        private void SubRefresh(object id)
        {
            subQuery = "select SalesCompositions.* " +
                          "from SalesCompositions " +
                          "left join Sales on Sales.Id = SalesCompositions.Sale_Id " +
                          "inner join Clients on Sales.Client_Id = Clients.Id " +
                          $"where Clients.id = {(int)id}";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    adapter = new SqlDataAdapter(subQuery, connection);
                    Sales = new DataTable();
                    adapter.Fill(Sales);
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
            //new SQLClient().UpdateClient(_Client);
            CanButtonClick = true;
        }
        #endregion

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;
            ClientAdd clientAdd = new ClientAdd();
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
           // Clients = new SQLClient().SelectClientToObservableCollection();
            CanButtonClick = true;
        }
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand { get; set; }

        private bool CanDeleteExecute(object p) => CanButtonClick;
        private void OnDeleteExecuted(object p)
        {
            if(SelectedClient != null)
            {
                CanButtonClick = false;
                //new SQLClient().DeleteClient(SelectedClient);
                CanButtonClick = true;
                OnUpdateExecuted(null);
            }
        }
        #endregion

        #endregion

        //public ObservableCollection<Clients> Clients { get => _Client; set => Set(ref _Client, value); }

        //ObservableCollection<Clients> _Client = new SQLClient().SelectClientToObservableCollection();

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
