namespace Dadrass.Dev.Cron.ExpressionParser.Core;

/// <summary>
/// Parses and evaluates expressions.
/// </summary>
public class Parser {
    readonly List<Token> _tokens;
    int _currentIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="Parser"/> class.
    /// </summary>
    /// <param name="tokens">The list of tokens to parse.</param>
    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _currentIndex = 0;
    }

    /// <summary>
    /// Parses and evaluates the entire expression.
    /// </summary>
    /// <returns>The result of the evaluated expression.</returns>
    public object ParseExpression()
    {
        return ParseLogicalOr();
    }

    object ParseLogicalOr()
    {
        var left = ParseEquality();

        while (Match(LogicalTokenType.Or))
        {
            var right = ParseEquality();
            left = (bool)left || (bool)right;
        }

        return left;
    }

    object ParseEquality()
    {
        var left = ParseComparison();

        while (Match(ComparisonTokenType.EqualEqual, ComparisonTokenType.NotEqual))
        {
            var operatorToken = Previous();
            var right = ParseComparison();

            left = operatorToken.ComparisonType switch
            {
                ComparisonTokenType.EqualEqual => Equals(left, right),
                ComparisonTokenType.NotEqual => !Equals(left, right),
                _ => throw new Exception($"Unexpected token: {operatorToken}")
            };
        }

        return left;
    }

    object ParseComparison()
    {
        var left = ParseAddition();

        while (Match(ComparisonTokenType.Less, ComparisonTokenType.LessEqual, ComparisonTokenType.Greater, ComparisonTokenType.GreaterEqual))
        {
            var operatorToken = Previous();
            var right = ParseAddition();

            left = operatorToken.ComparisonType switch
            {
                ComparisonTokenType.Less => (dynamic)left < (dynamic)right,
                ComparisonTokenType.LessEqual => (dynamic)left <= (dynamic)right,
                ComparisonTokenType.Greater => (dynamic)left > (dynamic)right,
                ComparisonTokenType.GreaterEqual => (dynamic)left >= (dynamic)right,
                _ => throw new Exception($"Unexpected token: {operatorToken}")
            };
        }

        return left;
    }

    object ParseAddition()
    {
        var left = ParseMultiplication();

        while (Match(ArithmeticTokenType.Plus, ArithmeticTokenType.Minus))
        {
            var operatorToken = Previous();
            var right = ParseMultiplication();

            left = operatorToken.ArithmeticType switch
            {
                ArithmeticTokenType.Plus => (dynamic)left + (dynamic)right,
                ArithmeticTokenType.Minus => (dynamic)left - (dynamic)right,
                _ => throw new Exception($"Unexpected token: {operatorToken}")
            };
        }

        return left;
    }

    object ParseMultiplication()
    {
        var left = ParsePrimary();

        while (Match(ArithmeticTokenType.Star, ArithmeticTokenType.Slash))
        {
            var operatorToken = Previous();
            var right = ParsePrimary();

            left = operatorToken.ArithmeticType switch
            {
                ArithmeticTokenType.Star => (dynamic)left * (dynamic)right,
                ArithmeticTokenType.Slash => (dynamic)left / (dynamic)right,
                _ => throw new Exception($"Unexpected token: {operatorToken}")
            };
        }

        return left;
    }

    object ParsePrimary()
    {
        if (Match(LiteralTokenType.True)) return true;
        if (Match(LiteralTokenType.False)) return false;
        if (Match(LiteralTokenType.Number)) return Previous().Literal;
        if (Match(LiteralTokenType.String)) return Previous().Literal;
        if (Match(OtherTokenType.LeftParen))
        {
            var expr = ParseExpression();
            Consume(OtherTokenType.RightParen, "Expected ')' after expression.");
            return expr;
        }

        throw new Exception($"Unexpected token: {Peek()}");
    }

    bool Match(params Enum[] tokenTypes)
    {
        foreach (var type in tokenTypes)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Checks if the current token matches the specified type.
    /// </summary>
    /// <param name="type">The token type to check.</param>
    /// <returns>True if the token matches, false otherwise.</returns>c
    public bool Check(Enum type)
    {
        if (IsAtEnd()) return false;

        // Check if the token matches the type of the expected category
        var currentToken = Peek();

        return type switch
        {
            ArithmeticTokenType tokenType => currentToken.ArithmeticType == tokenType,
            LogicalTokenType tokenType => currentToken.LogicalType == tokenType,
            ComparisonTokenType tokenType => currentToken.ComparisonType == tokenType,
            LiteralTokenType tokenType => currentToken.LiteralType == tokenType,
            OtherTokenType tokenType => currentToken.OtherType == tokenType,
            _ => false
        };

    }


    Token Advance()
    {
        if (!IsAtEnd()) _currentIndex++;
        return Previous();
    }

    /// <summary>
    /// Checks if the current token is the end of the input.
    /// </summary>
    /// <returns>True if it's the end of input, false otherwise.</returns>
    public bool IsAtEnd() => Peek().OtherType == OtherTokenType.Eof;
    Token Peek() => _tokens[_currentIndex];

    Token Previous() => _tokens[_currentIndex - 1];

    Token Consume(Enum type, string errorMessage)
    {
        if (Check(type)) return Advance();
        throw new Exception(errorMessage);
    }
}
