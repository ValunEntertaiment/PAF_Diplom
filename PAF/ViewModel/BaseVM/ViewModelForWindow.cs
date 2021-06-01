using PAF.Commands.Base;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PAF.ViewModel
{
    public class ViewModelForWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        /// <summary>Устанавливает новое значение свойства если оно отличается от текущего</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filed">Текущее значение</param>
        /// <param name="value">Новое значение</param>
        /// <returns>true новое значение было внесено</returns>
        protected virtual bool Set<T>(ref T filed, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(filed, value)) return false;
            filed = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        #region Commands

        #region CloseCommand
        public Action CloseAction { get; set; }
        public ICommand CloseCommand { get; set; }
        private bool CanCloseExecute(object p) => true;
        private void OnCloseExecuted(object p)
        {
            CloseAction();
        }
        #endregion

        #region MinimizeCommand
        public Action MinimizeAction { get; set; }
        public ICommand MinimizeCommand { get; set; }
        private bool CanMinimizeExecute(object p) => true;
        private void OnMinimizeExecuted(object p)
        {
            MinimizeAction();
        }
        #endregion

        #region FullscreenCommand
        public Action FullscreenAction { get; set; }
        public ICommand FullscreenCommand { get; set; }
        private bool CanFulscreenExecute(object p) => true;
        private void OnFulscreenExecuted(object p)
        {
            FullscreenAction();
        }
        #endregion
        #endregion

        public ViewModelForWindow()
        {
            CloseCommand = new LambdaCommand(OnCloseExecuted, CanCloseExecute);
            MinimizeCommand = new LambdaCommand(OnMinimizeExecuted, CanMinimizeExecute);
            FullscreenCommand = new LambdaCommand(OnFulscreenExecuted, CanFulscreenExecute);
        }
    }
}