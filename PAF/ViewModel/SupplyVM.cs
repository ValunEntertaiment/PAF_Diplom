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
    class SupplyVM : ViewModelForWindow, IPage
    {
        public DataTable DataTable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        Supplies _AddSupplies = new Supplies();
        /// <summary>Данные нового клиента</summary>
        public Supplies AddSupplies { get => _AddSupplies; set => Set(ref _AddSupplies, value); }

        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }

        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            CanButtonClick = false;
            //new SQL().UpdateSupply(_Suppies);
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
            SupplyAdd supplyAdd = new SupplyAdd();
            supplyAdd.ShowDialog();
            CanButtonClick = true;
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            CanButtonClick = false;
            Supplies = new SQL().SelectSupply();
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

        public List<Supplies> Supplies { get => _Suppies; set => Set(ref _Suppies, value); }
        

        List<Supplies> _Suppies = new SQL().SelectSupply();

        public SupplyVM()
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
