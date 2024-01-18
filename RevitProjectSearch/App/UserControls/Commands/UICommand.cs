using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RevitProjectSearch.App.UserControls.Commands
{
    public class UICommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<object> _Execute { get; set; }
        public Predicate<object> _CanExecute { get; set; }


        public UICommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod)
        {
            _Execute = ExecuteMethod;
            _CanExecute = CanExecuteMethod;
        }


        public bool CanExecute(object parameter)
        {
            return _CanExecute(parameter);
        }


        public void Execute(object parameter)
        {
            _Execute(parameter);
        }
    }
}
