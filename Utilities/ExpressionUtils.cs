namespace Dadrass.Dev.Cron.Utilities;

/// <summary>
/// Provides utility methods for common operations used in parsing and evaluation.
/// </summary>
public static class ExpressionUtils {
    /// <summary>
    /// Converts a token's value to the appropriate type for comparison.
    /// </summary>
    /// <param name="tokenValue">The string value of the token.</param>
    /// <returns>The converted value as an object.</returns>
    public static object ConvertTokenValue(string tokenValue)
    {
        if (double.TryParse(tokenValue, out var numValue))
            return numValue;
        if (bool.TryParse(tokenValue, out var boolValue))
            return boolValue;
        return tokenValue;
    }
}
