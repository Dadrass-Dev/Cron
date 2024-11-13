namespace Dadrass.Dev.Cron.Expressions;

using Lexing;

/// <summary>
/// Represents a binary operation with two operands and an operator, such as 'a + b' or 'x > y'.
/// </summary>
public class BinaryExpression : ExpressionBase {
    /// <summary>Gets the left operand expression.</summary>
    public ExpressionBase Left { get; }

    /// <summary>Gets the right operand expression.</summary>
    public ExpressionBase Right { get; }

    /// <summary>Gets the operator that defines the binary operation.</summary>
    public TokenType Operator { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
    /// </summary>
    /// <param name="left">The left operand expression.</param>
    /// <param name="right">The right operand expression.</param>
    /// <param name="inputOperator">The operator that defines the binary operation.</param>
    public BinaryExpression(ExpressionBase left, ExpressionBase right, TokenType inputOperator)
    {
        Left = left;
        Right = right;
        Operator = inputOperator;
    }
}
