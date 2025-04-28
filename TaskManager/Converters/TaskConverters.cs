using System;
using System.Globalization;

namespace TaskMaster.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string priority)
            {
                return priority switch
                {
                    "basse" => Colors.Green,
                    "moyenne" => Colors.Orange,
                    "haute" => Colors.OrangeRed,
                    "critique" => Colors.Red,
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DeadlineToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime deadline)
            {
                var today = DateTime.Today;
                var daysUntilDeadline = (deadline.Date - today).TotalDays;

                if (daysUntilDeadline < 0)
                {
                    return Colors.Red; // En retard
                }
                else if (daysUntilDeadline <= 1)
                {
                    return Colors.OrangeRed; // Aujourd'hui ou demain
                }
                else if (daysUntilDeadline <= 3)
                {
                    return Colors.Orange; // Dans les 3 jours
                }
                else
                {
                    return Colors.Green; // Plus de 3 jours
                }
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status switch
                {
                    "à faire" => Colors.Gray,
                    "en cours" => Colors.Blue,
                    "terminée" => Colors.Green,
                    "annulée" => Colors.Red,
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 