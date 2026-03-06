using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Folio.Converters;

/// <summary>
/// Converts a DateTime to a short month+year label.
/// Used by the mini calendar header binding.
/// </summary>
public sealed class CalendarDayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is DateTime dt ? dt.ToString("MMMM yyyy", culture) : string.Empty;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
