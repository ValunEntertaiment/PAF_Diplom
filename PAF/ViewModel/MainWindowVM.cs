using PAF.Commands.Base;
using PAF.View.Pages;
using PAF.View.Windows;
using PAF.ViewModel.BaseVM;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class MainWindowVM : ViewModelForWindow
    {
        #region Properties

        public string Search 
        { 
            get => _Search; 
            set 
            {
                Set(ref _Search, value);
                List<string> vs = new List<string>();
                foreach (var item in Page.DataTable.Select())
                {
                    string row ="";
                    foreach (var item1 in item.ItemArray)
                    {
                        row += (item1 + " ");
                    }
                    vs.Add(row);
                }

                var q = from row in vs
                        .Where(p => p.Contains(Search))
                        select row;
            }
        }
        string _Search;

        public IPage Page { get => _Page; set => Set(ref _Page, value); }
        static IPage _Page = new ComponentVM();

        /// <summary> основная таблица </summary>
        public Page ContentPage { get => _ContentPage; set => Set(ref _ContentPage, value); }
        Page _ContentPage = new Component(_Page);

        public int Width { get => _Width; set => Set(ref _Width, value); }
        static int _Width = 1150;

        public int Height { get => _Height; set => Set(ref _Height, value); }
        static int _Height = 600;
        #endregion

        #region Commands

        #region LogoutCommand
        public ICommand LogoutCommand { get; set; }
        private bool CanLogoutExecute(object p) => true;
        private void OnLogoutExecuted(object p)
        {
            new Login().Show();
            CloseAction();
            // Сделать переход на форму авторизации
        }
        #endregion

        #region ConnectionCommand
        public ICommand ConnectionCommand { get; set; }
        private bool CanConnectionExecute(object p) => true;
        private void OnConnectionExecuted(object p)
        {
            // Строка подключения
        }
        #endregion

        #region SelectClients
        public ICommand SelectClients { get; set; }
        private bool CanSClientsExecute(object p) => true;
        private void OnSClientsExecuted(object p)
        {
            Page = new ClientVM();
            ContentPage = new Client(_Page);
        }
        #endregion

        #region SelectSales
        public ICommand SelectSales { get; set; }
        private bool CanSelectSalesExecute(object p) => true;
        private void OnSelectSalesExecuted(object p)
        {
            Page = new SalayVM();
            ContentPage = new Salay(Page);
        }
        #endregion

        #region SelectEmployees
        public ICommand SelectEmployees { get; set; }
        private bool CanSEmployeesExecute(object p) => true;
        private void OnSEmployeesExecuted(object p)
        {
            Page = new EmployeeVM();
            ContentPage = new Employee(Page);
        }
        #endregion

        #region SelectDeliveries
        public ICommand SelectDeliveries { get; set; }
        private bool CanSelectDeliveriesExecute(object p) => true;
        private void OnSelectDeliveriesExecuted(object p)
        {
            Page = new DeliveryVM();
            ContentPage = new Delivery(Page);
        }
        #endregion

        #region SelectComponents
        public ICommand SelectComponents { get; set; }
        private bool CanSelectComponentsExecute(object p) => true;
        private void OnSelectComponentsExecuted(object p)
        {
            Page = new ComponentVM();
            ContentPage = new Component(Page);
        }
        #endregion

        #region SelectTypes
        public ICommand SelectTypes { get; set; }
        private bool CanSelectTypesExecute(object p) => true;
        private void OnSelectTypesExecuted(object p)
        {
            Page = new TypeVM();
            ContentPage = new View.Pages.Type(Page);
        }
        #endregion

        #region SelectSupplies
        public ICommand SelectSupplies { get; set; }
        private bool CanSelectSuppliesExecute(object p) => true;
        private void OnSelectSuppliesExecuted(object p)
        {
            Page = new SupplyVM();
            ContentPage = new Supply(Page);
        }
        #endregion
        #endregion

        public MainWindowVM()
        {
            #region Commands
            LogoutCommand = new LambdaCommand(OnLogoutExecuted, CanLogoutExecute);
            ConnectionCommand = new LambdaCommand(OnConnectionExecuted, CanConnectionExecute);
            
            SelectClients = new LambdaCommand(OnSClientsExecuted, CanSClientsExecute);
            SelectSales = new LambdaCommand(OnSelectSalesExecuted, CanSelectSalesExecute);
            SelectEmployees = new LambdaCommand(OnSEmployeesExecuted, CanSEmployeesExecute);
            SelectDeliveries = new LambdaCommand(OnSelectDeliveriesExecuted, CanSelectDeliveriesExecute);
            SelectComponents = new LambdaCommand(OnSelectComponentsExecuted, CanSelectComponentsExecute);
            SelectTypes = new LambdaCommand(OnSelectTypesExecuted, CanSelectTypesExecute);
            SelectSupplies = new LambdaCommand(OnSelectSuppliesExecuted, CanSelectSuppliesExecute);
            #endregion

            IPage page;
            page = new ComponentVM();
        }
    }
}