using PAF.Commands.Base;
using PAF.Data;
using PAF.Data.Entityies;
using PAF.ViewModel.BaseVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class TypeVM : ViewModel, IPage
    {
        #region Properties
        public DataTable DataTable { get => _DataTable; set => Set(ref _DataTable, value); }
        DataTable _DataTable;

        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;
        #endregion


        public void Refresh()
        {
            string query = "SELECT " +
                               "Id Код, " +
                               "[Name] Тип " +
                               //"ShortName 'Сокращенное название' " +
                           "FROM Types";
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
                MessageBox.Show(x.Message, "Type");
            }
        }

        public void Refresh(string search)
        {
            string query = "SELECT " +
                               "Id Код, " +
                               "[Name] Тип " +
                               //"ShortName 'Сокращенное название' " +
                           "FROM Types " +
                           $"where convert(varchar(max),Id) +' '+ convert(varchar(max),[Name]) +' '+ convert(varchar(max),ShortName) like '%{search}%' ";
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
                MessageBox.Show(x.Message, "Type",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void Upload()
        {
            try
            {
                string query = "SELECT " +
                                   "Id код, " +
                                   "[Name] Тип " +
                                   //"ShortName 'Короткое название' " +
                               "FROM Types";
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
            Upload();
        }
        #endregion

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            MessageBox.Show("Hello");
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            Refresh();
        }
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand { get; set; }
        private bool CanDeleteExecute(object p) => CanButtonClick;
        private void OnDeleteExecuted(object p)
        {
            //Types = new SQL().SelectType();
        }
        #endregion
        #endregion

        public TypeVM()
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
