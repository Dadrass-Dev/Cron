namespace Dadrass.Dev.Cron.Evaluation;

using Parsing;

/// <summary>
/// Resolves field values from a data source based on field names.
/// </summary>
public class DataSourceResolver {
    readonly Func<string, object> _dataSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSourceResolver"/> class.
    /// </summary>
    /// <param name="dataSource">A function to retrieve values by field name.</param>
    public DataSourceResolver(Func<string, object> dataSource)
    {
        _dataSource = dataSource;
    }

    /// <summary>
    /// Retrieves the value of a field by its name.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>The value of the field as an object.</returns>
    public object GetFieldValue(string fieldName)
    {
        try
        {
            return _dataSource(fieldName);
        }
        catch (Exception ex)
        {
            throw new ParseException($"Error resolving field '{fieldName}': {ex.Message}", ex);
        }
    }
}
