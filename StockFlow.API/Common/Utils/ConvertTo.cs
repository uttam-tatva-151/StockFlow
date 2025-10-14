using System.Globalization;

namespace StockFlow.Common.Utils;
public static class ConvertTo
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="readField"></param>
    /// <returns></returns>
    public static string String(object readField)
    {
        if (readField == null || readField.GetType() == typeof(DBNull))
            return string.Empty;

        return Convert.ToString(readField, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="readField"></param>
    /// <returns></returns>
    public static decimal Decimal(object? readField)
    {
        if (readField == null || readField.GetType() == typeof(DBNull))
            return 0;

        if (readField?.ToString()?.Trim().Length == 0)
        {
            return 0;
        }
        else
        {
            if (decimal.TryParse(readField?.ToString(), out decimal x))
            {
                x = decimal.Round(x, 2);
                return x;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="readField"></param>
    /// <returns></returns>
    public static bool Boolean(object readField)
    {
        if (readField == null || readField.GetType() == typeof(DBNull))
            return false;

        if (bool.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out bool x))
        {
            return x;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string? CapitalizeFirstLetter(this string? str)
    {
        if (string.IsNullOrWhiteSpace(str)) return null;
        return $"{char.ToUpper(str[0])}{str[1..]}";
    }
}
