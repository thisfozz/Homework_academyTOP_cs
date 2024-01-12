using System.Windows.Input;

namespace VegetableFruitApp.Command
{
    public abstract class Command : ICommand
    {
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }

        protected virtual bool CanExecute(object parameter)
        {
            return true;
        }

        protected abstract void Execute(object parameter = null);

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
