namespace Dadrass.Dev.Cron.ExpressionParser.Core;

/// <summary>
/// Evaluates expressions parsed by the Parser.
/// </summary>
public class ExpressionEvaluator
{
    readonly Dictionary<string, object> _parameters;
    readonly Dictionary<string, object> _fields;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionEvaluator"/> class.
    /// </summary>
    /// <param name="parameters">Input parameters for the evaluation.</param>
    /// <param name="fields">Data fields for the evaluation.</param>
    public ExpressionEvaluator(Dictionary<string, object> parameters, Dictionary<string, object> fields)
    {
        _parameters = parameters;
        _fields = fields;
    }

    /// <summary>
    /// Resolves an identifier to its value from parameters or fields.
    /// </summary>
    /// <param name="identifier">The identifier name.</param>
    /// <returns>The resolved value.</returns>
    public object ResolveIdentifier(string identifier)
    {
        if (_parameters.TryGetValue(identifier, out var paramValue))
            return paramValue;

        if (_fields.TryGetValue(identifier, out var fieldValue))
            return fieldValue;

        throw new Exception($"Unknown identifier: {identifier}");
    }

    /// <summary>
    /// Evaluates a binary operation between two values.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <param name="operator">The operator (+, -, *, etc.).</param>
    /// <returns>The result of the operation.</returns>
    public object EvaluateBinaryOperation(object left, object right, string @operator)
    {
        return @operator switch
        {
            "+" => (int)left + (int)right,
            "-" => (int)left - (int)right,
            "*" => (int)left * (int)right,
            "/" => (int)left / (int)right,
            "&&" => (bool)left && (bool)right,
            "||" => (bool)left || (bool)right,
            _ => throw new Exception($"Unsupported operator: {@operator}")
        };
    }
}
