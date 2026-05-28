
using System.Globalization;

namespace ReceiptReader.Presentation.Converters;

public class StringToSolidColorBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string ArgbHex)
            return new SolidColorBrush(Color.FromArgb(ArgbHex));
        return new SolidColorBrush(Colors.Gray);
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
