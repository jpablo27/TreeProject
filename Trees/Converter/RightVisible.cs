// -----------------------------------------------------------------------
// <author>Pablo Sánchez</author>
// <date>2022-09-15</date>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Trees.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using Trees.Enums;

    public class RightVisible : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            if (value is Side side && side == Side.Right)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}