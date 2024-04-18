using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoEGraveList.Commands
{
    class BaseCommand : ICommand
    {
        private readonly Action<object?> _executeAction;
        public event EventHandler? CanExecuteChanged;

        public BaseCommand(Action<object?> executeAction)
        {
            this._executeAction = executeAction;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            this._executeAction(parameter);
        }
    }
}
