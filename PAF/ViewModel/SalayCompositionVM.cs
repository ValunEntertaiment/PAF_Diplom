﻿using PAF.Commands.Base;
using PAF.Data;
using PAF.Data.Entityies;
using PAF.View.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class SalayCompositionVM : ViewModelForWindow
    {
        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        SalesCompositions _AddSalesCompositions = new SalesCompositions();
        /// <summary>Данные нового товара</summary>
        public SalesCompositions AddSalesCompositions { get => _AddSalesCompositions; set => Set(ref _AddSalesCompositions, value); }

        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }

        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            CanButtonClick = false;
            //new SQL().UpdateSalayComposition(_SalesCompositions);
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
            SalayCompositionAdd salayCompositionAdd = new SalayCompositionAdd();
            salayCompositionAdd.ShowDialog();
            CanButtonClick = true;
        }
        #endregion

        #region UpdateCommand
        public ICommand UpdateCommand { get; set; }
        private bool CanUpdateExecute(object p) => CanButtonClick;
        private void OnUpdateExecuted(object p)
        {
            CanButtonClick = false;
            //SalesCompositions = new SQL().SelectSalayComposition();
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

        public List<SalesCompositions> SalesCompositions { get => _SalesCompositions; set => Set(ref _SalesCompositions, value); }
        List<SalesCompositions> _SalesCompositions = new SQL().SelectSalayComposition();

        public SalayCompositionVM()
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
