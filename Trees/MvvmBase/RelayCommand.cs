// -----------------------------------------------------------------------
// <author>Pablo Sánchez</author>
// <date>2022-09-14</date>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Trees.MvvmBase;

using System;
using System.Windows.Input;

public class RelayCommand<T> : ICommand, IDisposable
{
    #region Fields

    /// <summary>
    /// The _can execute.
    /// </summary>
    private Predicate<T> canExecute;

    /// <summary>
    /// The execute.
    /// </summary>
    private Action<T> execute;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}" /> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    public RelayCommand(Action<T> execute)
        : this(execute, null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="RelayCommand{T}" /> class.</summary>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    /// <exception cref="ArgumentNullException"> Not implemented </exception>
    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    #endregion

    #region Public Events

    /// <summary>
    /// The can execute changed.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add
        {
            if (this.canExecute != null)
            {
                CommandManager.RequerySuggested += value;
            }
        }

        remove
        {
            if (this.canExecute != null)
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// The can execute.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns><c>True</c> on success.</returns>
    public bool CanExecute(object parameter)
    {
        return this.canExecute == null || this.canExecute((T)parameter);
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    public void Dispose()
    {
        this.execute = null;
        this.canExecute = null;
    }

    /// <summary>
    /// The execute.
    /// </summary>
    /// <param name="parameter">
    /// The parameter.
    /// </param>
    public void Execute(object parameter)
    {
        this.execute((T)parameter);
    }

    #endregion
}

/// <summary>
/// Class RelayCommand.
/// Implements the <see cref="ICommand" />
/// Implements the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="ICommand" />
/// <seealso cref="IDisposable" />
public class RelayCommand : ICommand, IDisposable
{
    #region Fields

    /// <summary>
    /// The can execute
    /// </summary>
    private Func<bool> canExecute;

    /// <summary>
    /// The execute
    /// </summary>
    private Action execute;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand" /> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    /// <exception cref="System.ArgumentNullException">execute</exception>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    #endregion

    #region Public Events

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
        }

        remove
        {
            CommandManager.RequerySuggested -= value;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command.  If the command does not require data to be passed, this object can
    /// be set to <see langword="null" />.
    /// </param>
    /// <returns><see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.</returns>
    public bool CanExecute(object parameter)
    {
        return this.canExecute == null || this.canExecute();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        this.execute = null;
        this.canExecute = null;
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command.  If the command does not require data to be passed, this object can
    /// be set to <see langword="null" />.
    /// </param>
    public void Execute(object? parameter)
    {
        this.execute();
    }

    #endregion
}