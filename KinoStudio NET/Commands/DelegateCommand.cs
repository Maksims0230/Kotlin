using System;
using System.Windows.Input;

namespace KinoStudio_NET.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null) =>
            (this._execute, this._canExecute) = (execute, canExecute);

        public bool CanExecute(object parameter) => this._canExecute == null || this._canExecute(parameter);

        public void Execute(object parameter) => this._execute(parameter);
    }
}