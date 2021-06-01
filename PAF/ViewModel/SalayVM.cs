using PAF.Commands.Base;
using PAF.Data;
using PAF.Data.Entityies;
using PAF.View.Windows;
using PAF.ViewModel.BaseVM;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class SalayVM : ViewModelForWindow, IPage
    {
        public DataTable DataTable { get; set; }

        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        Deliveries _AddSalay = new Deliveries();
        /// <summary>Данные нового клиента</summary>
        public Deliveries AddSales { get => _AddSalay; set => Set(ref _AddSalay, value); }

        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }

        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            CanButtonClick = false;
            //new SQL().UpdateSalay(_Sales);
            CanButtonClick = true;
        }
        #endregion

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;
            //AddClient = new Clients();
            SalayAdd salayAdd = new SalayAdd();
            salayAdd.ShowDialog();
            CanButtonClick = true;
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            CanButtonClick = false;
           // Sales = new SQL().SelectSalay();
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

        #region AddClientCommand
        public ICommand AddClientCommand { get; set; }
        private bool CanAddClientExecute(object p) => CanButtonClick;
        private void OnAddClientExecuted(object p)
        {
            CanButtonClick = false;
            //MessageBox.Show(AddClient.FirstName + " " + AddClient.LastName + " " + AddClient.MiddleName);
            CanButtonClick = true;
        }
        #endregion
        #endregion

        public List<Sales> Sales { get => _Sales; set => Set(ref _Sales, value); }

        List<Sales> _Sales = new SQL().SelectSalay();

        public SalayVM()
        {
            #region Commands
            SaveChangesCommand = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            AddCommand = new LambdaCommand(OnAddExecuted, CanAddExecute);
            UpdateCommand = new LambdaCommand(OnUpdateExecuted, CanUpdateExecute);
            DeleteCommand = new LambdaCommand(OnDeleteExecuted, CanDeleteExecute);
            AddClientCommand = new LambdaCommand(OnAddClientExecuted, CanAddClientExecute);
            #endregion
        }
    }
}