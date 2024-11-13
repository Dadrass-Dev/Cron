namespace Dadrass.Dev.Cron.Evaluation;

using Expressions;
using Lexing;

/// <summary>
/// Evaluates parsed expressions by resolving field values from the data source and applying logical and comparison operations.
/// </summary>
public class Evaluator {
    readonly Func<string, object> _dataSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="Evaluator"/> class.
    /// </summary>
    /// <param name="dataSource">A function to retrieve values by field name for evaluation.</param>
    public Evaluator(Func<string, object> dataSource)
    {
        _dataSource = dataSource;
    }

    /// <summary>
    /// Evaluates a logical expression and returns its boolean result.
    /// </summary>
    /// <param name="expression">The logical expression to evaluate.</param>
    /// <returns>True if the expression evaluates as true, otherwise false.</returns>
    public bool EvaluateLogicalExpression(ExpressionBase expression)
    {
        switch (expression)
        {
            case LogicalExpression logical:
                var left = EvaluateLogicalExpression(logical.Left);
                var right = EvaluateLogicalExpression(logical.Right);
                return logical.Operator == TokenType.And ? left && right : left || right;

            case ComparisonExpression comparison:
                var leftValue = _dataSource(comparison.LeftField);
                var rightValue = comparison.RightValue;
                return EvaluateComparison(leftValue, rightValue, comparison.Operator);

            default:
                throw new Exception("Unsupported expression type");
        }
    }

    /// <summary>
    /// Compares two values using the specified comparison operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="operation">The comparison operation to apply.</param>
    /// <returns>The result of the comparison operation.</returns>
    bool EvaluateComparison(object left, object right, TokenType operation)
    {
        return operation switch
        {
            TokenType.Equal => left.Equals(right),
            TokenType.NotEqual => !left.Equals(right),
            TokenType.GreaterThan => (double)left > (double)right,
            TokenType.LessThan => (double)left < (double)right,
            _ => throw new Exception("Unsupported comparison operation"),
        };
    }
}
