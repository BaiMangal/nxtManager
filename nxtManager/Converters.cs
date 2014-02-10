using nxtAPIwrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace nxtManager.Converters
{
    public class InvertedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is Visibility)
            {
                if ((Visibility)value == Visibility.Collapsed)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TimestampToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value == String.Empty)
                return DateTime.MinValue;

            var timestamp = Double.Parse(value.ToString());
            return new DateTime(1970, 1, 1).AddSeconds(timestamp);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valueToReturn = Visibility.Collapsed;
            if (value == null || value is List<Alias> == false)
                return Visibility.Collapsed;

            if (((List<Alias>)value).Count() > 0)
                valueToReturn = Visibility.Visible;


            if (parameter != null && parameter.ToString() == "inverse")
            {
                if (valueToReturn == Visibility.Collapsed)
                    valueToReturn = Visibility.Visible;
                else
                    valueToReturn = Visibility.Collapsed;
            }
            return valueToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertedBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is Boolean)
            {
                return !((Boolean)value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PeerStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is int && parameter != null && !String.IsNullOrEmpty(parameter.ToString()))
            {
                if ((int)value == 1 && parameter.ToString() == "connected")
                    return Visibility.Visible;
                else if ((int)value == 0 && parameter.ToString() == "disconnected")
                    return Visibility.Visible;
                else if ((int)value == 2 && parameter.ToString() == "disconnected")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PeerWeightToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is double && parameter != null && !String.IsNullOrEmpty(parameter.ToString()))
            {
                if ((double)value > 0 && parameter.ToString() == "positive")
                    return Visibility.Visible;
                else if ((double)value == 0 && parameter.ToString() == "zero")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PeerAddressToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is string && parameter != null && !String.IsNullOrEmpty(parameter.ToString()) && App.DVM != null && App.DVM.WellKnownPeers != null)
            {
                if (App.DVM.WellKnownPeers.Contains((string)value) && parameter.ToString() == "wellKnown")
                    return Visibility.Visible;
                else if (!(App.DVM.WellKnownPeers.Contains((string)value)) && parameter.ToString() == "regular")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TransactionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is Transaction && parameter != null && !String.IsNullOrEmpty(parameter.ToString()) && App.DVM != null && App.DVM.NXTAcc != null)
            {
                if (App.DVM.NXTAcc.accountId == ((Transaction)value).sender && ((Transaction)value).IsSenderEqualToRecipient && parameter.ToString() == "myself")
                    return Visibility.Visible;
                else if (App.DVM.NXTAcc.accountId == ((Transaction)value).sender && ((Transaction)value).IsSenderEqualToRecipient == false && parameter.ToString() == "recipient")
                    return Visibility.Visible;
                else if (App.DVM.NXTAcc.accountId == ((Transaction)value).recipient && ((Transaction)value).IsSenderEqualToRecipient == false && parameter.ToString() == "sender")
                    return Visibility.Visible;
                else if (App.DVM.NXTAcc.accountId == ((Transaction)value).sender && parameter.ToString() == "sent")
                    return Visibility.Visible;
                else if (App.DVM.NXTAcc.accountId == ((Transaction)value).recipient && parameter.ToString() == "received")
                    return Visibility.Visible;
                else
                {
                    if (parameter.ToString() == "sent" || parameter.ToString() == "received")
                        return Visibility.Hidden;
                    else
                        return Visibility.Collapsed;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "inverted")
                value = !(bool)value;
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
