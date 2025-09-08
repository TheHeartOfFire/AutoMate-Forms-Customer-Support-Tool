using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace AMFormsCST.Desktop.Converters;

public class ContentFallbackConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 3 ||
        values[0] == DependencyProperty.UnsetValue ||
        values[1] == DependencyProperty.UnsetValue)
        {
            return values.Length >= 3 ? values[2] : null;
        }

        object dataItem = values[0];
        string bindingPath = values[1] as string;
        string fallbackValue = values[2] as string;

        if (dataItem == null || string.IsNullOrEmpty(bindingPath))
        {
            return fallbackValue;
        }


        if (dataItem is Models.Dealer dealer)
        {

            if (bindingPath == "CompositeDealerName")
            {
                string serverCode = GetPropertyValue<string>(dealer, nameof(dealer.ServerCode));
                string dealerName = GetPropertyValue<string>(dealer, nameof(dealer.Name));


                if (string.IsNullOrEmpty(serverCode) && string.IsNullOrEmpty(dealerName))
                {
                    return fallbackValue;
                }

                return $"({serverCode}){dealerName}";
            }
        }

        if (dataItem is Models.Company company) 
        {
           
            if (bindingPath == "CompositeCompanyName")
            {
                string companyCode = GetPropertyValue<string>(company, nameof(company.CompanyCode));
                string companyName = GetPropertyValue<string>(company, nameof(company.Name));

                bool hasCompanyCode = !string.IsNullOrEmpty(companyCode);
                bool hasCompanyName = !string.IsNullOrEmpty(companyName);

                
                if (!hasCompanyCode && !hasCompanyName)
                {
                    return fallbackValue;
                }
                
                else if (hasCompanyCode && hasCompanyName)
                {
                    return $"({companyCode}){companyName}";
                }
                
                else if (hasCompanyCode)
                {
                    return $"({companyCode})";
                }
                
                else 
                {
                    return companyName;
                }
            }
        }
        PropertyInfo prop = dataItem.GetType().GetProperty(bindingPath);
        if (prop != null)
        {
            object contentValue = prop.GetValue(dataItem);

            return (contentValue is string s && !string.IsNullOrEmpty(s)) || (contentValue != null && !(contentValue is string))
                ? contentValue
                : fallbackValue;
        }

        return fallbackValue;
    }



    private T GetPropertyValue<T>(object obj, string propertyName)
    {
        if (obj == null) return default(T);
        PropertyInfo prop = obj.GetType().GetProperty(propertyName);
        if (prop != null && prop.PropertyType == typeof(T))
        {
            return (T)prop.GetValue(obj);
        }
        return default(T);
    }


    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}


