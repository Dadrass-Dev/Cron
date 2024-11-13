namespace Dadrass.Dev.Cron.Parsing;

using Lexing;

/// <summary>
/// Parses logical and comparison expressions and evaluates them against a data source.
/// </summary>
public class Parser {
    readonly Lexer _lexer;
    Token _currentToken;
    readonly Func<string, object> _dataSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="Parser"/> class.
    /// </summary>
    /// <param name="lexer">The lexer that provides tokens for parsing.</param>
    /// <param name="dataSource">A function to access data fields by name.</param>
    public Parser(Lexer lexer, Func<string, object> dataSource)
    {
        _lexer = lexer;
        _currentToken = _lexer.GetNextToken();
        _dataSource = dataSource;
    }

    /// <summary>
    /// Parses and evaluates a logical expression.
    /// </summary>
    /// <returns>The result of the logical expression.</returns>
    public bool ParseLogicalExpression()
    {
        var left = ParseComparison();

        while (_currentToken.Type == TokenType.And || _currentToken.Type == TokenType.Or)
        {
            var operation = _currentToken.Type;
            _currentToken = _lexer.GetNextToken();
            var right = ParseComparison();

            left = operation == TokenType.And ? left && right : left || right;
        }

        return left;
    }

    /// <summary>
    /// Parses a comparison expression, such as 'age > 18'.
    /// </summary>
    /// <returns>The result of the comparison.</returns>
    bool ParseComparison()
    {
        var leftValue = ParsePrimary();

        if (_currentToken.Type == TokenType.GreaterThan || _currentToken.Type == TokenType.LessThan ||
            _currentToken.Type == TokenType.Equal || _currentToken.Type == TokenType.NotEqual)
        {
            var operation = _currentToken.Type;
            _currentToken = _lexer.GetNextToken();
            var rightValue = ParsePrimary();

            return EvaluateComparison(leftValue, rightValue, operation);
        }

        throw new Exception("Invalid comparison expression");
    }

    /// <summary>
    /// Parses a primary expression, which may be a number, string, or identifier.
    /// </summary>
    /// <returns>The parsed primary value.</returns>
    object ParsePrimary()
    {
        if (_currentToken.Type == TokenType.Identifier)
        {
            var fieldName = _currentToken.Value;
            _currentToken = _lexer.GetNextToken();
            return _dataSource(fieldName);
        }
        if (_currentToken.Type == TokenType.Number)
        {
            var value = double.Parse(_currentToken.Value);
            _currentToken = _lexer.GetNextToken();
            return value;
        }
        if (_currentToken.Type == TokenType.String)
        {
            var value = _currentToken.Value;
            _currentToken = _lexer.GetNextToken();
            return value;
        }

        throw new Exception("Unexpected token");
    }

    /// <summary>
    /// Evaluates a comparison operation between two values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="operation">The comparison operation to perform.</param>
    /// <returns>The result of the comparison.</returns>
    bool EvaluateComparison(object left, object right, TokenType operation)
    {
        return operation switch
        {
            TokenType.Equal => left.Equals(right),
            TokenType.NotEqual => !left.Equals(right),
            TokenType.GreaterThan => (double)left > (double)right,
            TokenType.LessThan => (double)left < (double)right,
            _ => throw new Exception("Unsupported operation")
        };
    }
}
