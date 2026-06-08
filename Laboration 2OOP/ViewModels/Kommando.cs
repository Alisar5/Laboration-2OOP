
using System.Windows.Input;

namespace Laboration_2OOP.ViewModels
{
    public class Kommando : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public Kommando(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}