﻿using PAF.Commands.Base;
using PAF.Data;
using PAF.Data.Entityies;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class DeliveryCompositionVM : ViewModelForWindow
    {
        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        Deliveries _AddDelivery = new Deliveries();
        /// <summary>Данные нового клиента</summary>
        public Deliveries AddDelivery { get => _AddDelivery; set => Set(ref _AddDelivery, value); }

        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }

        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            CanButtonClick = false;
            new SQL().UpdateClient(_Clients);
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
            DeliveryCompositionAdd clientAdd = new DeliveryCompositionAdd();
            clientAdd.ShowDialog();
            CanButtonClick = true;
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            CanButtonClick = false;
            Clients = new SQL().SelectClient();
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

        public List<Clients> Clients { get => _Clients; set => Set(ref _Clients, value); }
        List<Clients> _Clients = new SQL().SelectClient();

        public DeliveryCompositionVM()
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