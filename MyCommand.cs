using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sweeper05
{
    public class MyICommand : ICommand
    {
        Action _TargetExecuteMethod;
        Func<bool> _TargetCanExecuteMethod; //the hell is this?


        //constructor takes either action or action and can method?
        public MyICommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }
        public MyICommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }



        //needs the interface functions?
        public bool CanExecute(object parameter)
        {
            //if can exists, return method,
            //if execute exists, return true,
            //otherwise false
            if (_TargetCanExecuteMethod != null)
                return _TargetCanExecuteMethod();
            if (_TargetExecuteMethod != null)
                return true;
            return false;
        }

        /* tut says something about weak references 
         * if command instance lifetime is longer than
         * lifetime of UI objects hooked up to command
         */

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
                _TargetExecuteMethod();
        }
    }
}
