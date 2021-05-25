﻿using PAF.Commands.Base;
using PAF.Data;
using PAF.Data.Clases;
using PAF.Data.Entityies;
using PAF.View.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class DeliveryVM : ViewModelForWindow
    {
        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        public Deliveries SelectedDelivery { get; set; }

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
            new SQLDelivery().Update(_Deliveries);
            CanButtonClick = true;
        }
        #endregion

        #region AddCommand
        public ICommand AddCommand { get; set; }
        private bool CanAddExecute(object p) => CanButtonClick;
        private void OnAddExecuted(object p)
        {
            CanButtonClick = false;
            DeliveryAdd deliveryAdd = new DeliveryAdd();
            deliveryAdd.ShowDialog();
            CanButtonClick = true;
            Deliveries = new SQLDelivery().Select();
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            CanButtonClick = false;
            Deliveries = new SQLDelivery().Select();
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

        public List<Deliveries> Deliveries { get => _Deliveries; set => Set(ref _Deliveries, value); }
        List<Deliveries> _Deliveries = new SQL().SelectDelivery();

        public DeliveryVM()
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
