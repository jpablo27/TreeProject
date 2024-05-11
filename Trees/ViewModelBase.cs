using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Trees;

/// <summary>
/// The view model base
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// Property changed event handler
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// On Property changed
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Refresh
    /// </summary>
    public void Refresh()
    {
        if (Application.Current.CheckAccess())
            OnPropertyChanged(string.Empty);
        else
            Application.Current.Dispatcher.Invoke(Refresh);
    }

    /// <summary>
    /// Set field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}