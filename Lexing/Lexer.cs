namespace Dadrass.Dev.Cron.Lexing;

/// <summary>
/// Tokenizes an input expression into individual tokens for parsing.
/// </summary>
public class Lexer {
    readonly string _expression;
    int _position;

    /// <summary>
    /// Initializes a new instance of the <see cref="Lexer"/> class.
    /// </summary>
    /// <param name="expression">The expression to tokenize.</param>
    public Lexer(string expression)
    {
        _expression = expression;
    }

    /// <summary>
    /// Retrieves the next token in the expression.
    /// </summary>
    /// <returns>The next <see cref="Token"/> in the expression.</returns>
    public Token GetNextToken()
    {
        // Skip whitespace
        while (_position < _expression.Length && char.IsWhiteSpace(_expression[_position]))
            _position++;

        if (_position >= _expression.Length)
            return new Token(TokenType.EndOfExpression);

        var current = _expression[_position];

        // Handle identifiers
        if (char.IsLetter(current))
        {
            var identifier = ParseIdentifier();
            if (identifier == "&&")
                return new Token(TokenType.And);
            if (identifier == "||")
                return new Token(TokenType.Or);
            return new Token(TokenType.Identifier, identifier);
        }

        // Handle operators
        if (current == '>' || current == '<' || current == '=' || current == '!')
        {
            var op = ParseOperator();
            return op switch
            {
                ">" => new Token(TokenType.GreaterThan),
                "<" => new Token(TokenType.LessThan),
                ">=" => new Token(TokenType.GreaterThanOrEqual),
                "<=" => new Token(TokenType.LessThanOrEqual),
                "==" => new Token(TokenType.Equal),
                "!=" => new Token(TokenType.NotEqual),
                _ => throw new Exception("Unknown operator"),
            };
        }

        // Additional token parsing...

        _position++;
        return new Token(TokenType.EndOfExpression);// Default case
    }

    /// <summary>
    /// Parses an identifier from the current position in the expression.
    /// </summary>
    /// <returns>The parsed identifier string.</returns>
    string ParseIdentifier()
    {
        var start = _position;
        while (_position < _expression.Length && char.IsLetterOrDigit(_expression[_position]))
            _position++;
        return _expression.Substring(start, _position - start);
    }

    /// <summary>
    /// Parses an operator from the current position in the expression.
    /// </summary>
    /// <returns>The parsed operator string.</returns>
    string ParseOperator()
    {
        var start = _position;
        while (_position < _expression.Length && (char.IsSymbol(_expression[_position]) || _expression[_position] == '='))
            _position++;
        return _expression.Substring(start, _position - start);
    }
}
