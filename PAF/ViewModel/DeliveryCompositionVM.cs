using PAF.Commands.Base;
using PAF.Data;
using PAF.Data.Clases;
using PAF.Data.Classes;
using PAF.Data.Entityies;
using PAF.View.Windows;
using PAF.ViewModel.BaseVM;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class DeliveryCompositionVM : ViewModelForWindow, IPage
    {
        public DataTable DataTable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        public DeliveriesCompositions Selecteditem { get; set; }

        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }

        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            CanButtonClick = false;
            new SQLDeliveryComposition().Update(_DeliveriesCompositions);
            CanButtonClick = true;
        }
        #endregion

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;
            DeliveryCompositionAdd clientAdd = new DeliveryCompositionAdd();
            clientAdd.ShowDialog();
            CanButtonClick = true;
            DeliveriesCompositions = new SQLDeliveryComposition().Select();
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            CanButtonClick = false;
            DeliveriesCompositions = new SQLDeliveryComposition().Select();
            CanButtonClick = true;
        }
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand { get; set; }
        private bool CanDeleteExecute(object p) => CanButtonClick;
        private void OnDeleteExecuted(object p)
        {
            CanButtonClick = false;
            new SQLDeliveryComposition().Delete(Selecteditem);
            CanButtonClick = true;
        }
        #endregion
        
        #endregion

        public List<DeliveriesCompositions> DeliveriesCompositions { get => _DeliveriesCompositions; set => Set(ref _DeliveriesCompositions, value); }
       

        List<DeliveriesCompositions> _DeliveriesCompositions = new SQL().SelectDeliveriesCompositions();

        public DeliveryCompositionVM()
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
