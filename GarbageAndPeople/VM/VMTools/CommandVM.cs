using System.Windows.Input;

namespace GarbageAndPeople.VM.VMTools
{
    public class CommandVM(Action action, Func<bool> canExecute) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        ;

        Action action = action;
        Func<bool> canExecute = canExecute;

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public void Execute(object? parameter)
        {
            action();
        }
    }
}
