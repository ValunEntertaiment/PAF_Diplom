using PAF.Commands.Base;
using PAF.Data;
using PAF.Data.Entityies;
using PAF.ViewModel.BaseVM;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace PAF.ViewModel
{
    class TypeVM : ViewModel, IPage
    {
        public DataTable DataTable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        /// <summary>Пока прога работает с бд, лучше запретить все кнопки для работы с бд</summary>
        bool CanButtonClick = true;

        #region Commands

        #region SaveChangesCommand
        public ICommand SaveChangesCommand { get; set; }
        private bool CanSaveChangesExecute(object p) => CanButtonClick;
        private void OnSaveChangesExecuted(object p)
        {
            new SQL().UpdateType(_Types);
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
            Types = new SQL().SelectType();
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

        public List<Types> Types { get => _Types; set => Set(ref _Types, value); }

        List<Types> _Types = new SQL().SelectType();

        public TypeVM()
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
