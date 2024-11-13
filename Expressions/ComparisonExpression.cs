namespace Dadrass.Dev.Cron.Expressions;

using Lexing;

/// <summary>
/// Represents a comparison expression, such as "field > 10".
/// </summary>
public class ComparisonExpression : ExpressionBase {
    /// <summary>Gets the field name being compared.</summary>
    public string LeftField { get; }

    /// <summary>Gets the value being compared to the field.</summary>
    public object RightValue { get; }

    /// <summary>Gets the comparison operator.</summary>
    public TokenType Operator { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComparisonExpression"/> class.
    /// </summary>
    /// <param name="leftField">The name of the field being compared.</param>
    /// <param name="rightValue">The value to compare the field against.</param>
    /// <param name="inputOperator">The comparison operator.</param>
    public ComparisonExpression(string leftField, object rightValue, TokenType inputOperator)
    {
        LeftField = leftField;
        RightValue = rightValue;
        Operator = inputOperator;
    }
}
