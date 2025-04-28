using System;
using System.Globalization;

namespace TaskMaster.Converters
{
    public class StatusToDecorationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status && status == "terminée")
            {
                return TextDecorations.Strikethrough;
            }
            return TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToNextActionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status switch
                {
                    "à faire" => "Démarrer",
                    "en cours" => "Terminer",
                    "terminée" => "Rouvrir",
                    _ => "Changer"
                };
            }
            return "Changer";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string firstName = value as string ?? string.Empty;
            string lastName = parameter as string ?? string.Empty;

            if (parameter is Binding binding)
            {
                // Si le paramètre est une liaison, on ne peut pas l'utiliser directement
                // Cette situation est gérée par XAML
                return firstName;
            }

            return $"{firstName} {lastName}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 