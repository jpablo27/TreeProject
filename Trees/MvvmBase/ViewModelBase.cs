namespace Trees.MvvmBase;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// Class ViewModelBase.
/// Implements the <see cref="INotifyPropertyChanged" />
/// </summary>
/// <seealso cref="INotifyPropertyChanged" />
public class ViewModelBase : INotifyPropertyChanged
{
    #region Public Events

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Public Methods

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    public void Refresh()
    {
        this.OnPropertyChanged(string.Empty);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Called when [property changed].
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}