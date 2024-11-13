namespace Dadrass.Dev.Cron.Expressions;

using Lexing;

/// <summary>
/// Represents a logical expression that combines two sub-expressions with a logical operator.
/// </summary>
public class LogicalExpression : ExpressionBase {
    /// <summary>Gets the left operand expression.</summary>
    public ExpressionBase Left { get; }

    /// <summary>Gets the right operand expression.</summary>
    public ExpressionBase Right { get; }

    /// <summary>Gets the logical operator applied to the operands.</summary>
    public TokenType Operator { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicalExpression"/> class.
    /// </summary>
    /// <param name="left">The left operand expression.</param>
    /// <param name="right">The right operand expression.</param>
    /// <param name="inputOperator">The logical operator.</param>
    public LogicalExpression(ExpressionBase left, ExpressionBase right, TokenType inputOperator)
    {
        Left = left;
        Right = right;
        Operator = inputOperator;
    }
}
