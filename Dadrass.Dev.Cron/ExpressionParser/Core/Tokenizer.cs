namespace Dadrass.Dev.Cron.ExpressionParser.Core;

/// <summary>
/// Tokenizes the input string into a sequence of tokens.
/// </summary>
public class Tokenizer {
    readonly string _input;
    int _currentIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tokenizer"/> class.
    /// </summary>
    /// <param name="input">The input string to tokenize.</param>
    public Tokenizer(string input)
    {
        _input = input;
        _currentIndex = 0;
    }

    /// <summary>
    /// Tokenizes the entire input string into a list of tokens.
    /// </summary>
    /// <returns>A list of tokens parsed from the input string.</returns>
    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (!IsAtEnd())
        {
            var c = Advance();// Get the current character and advance the index

            switch (c)
            {
                case '(':
                    tokens.Add(new Token(OtherTokenType.LeftParen, "("));
                    break;
                case ')':
                    tokens.Add(new Token(OtherTokenType.RightParen, ")"));
                    break;
                case '+':
                    tokens.Add(new Token(ArithmeticTokenType.Plus, "+"));
                    break;
                case '-':
                    tokens.Add(new Token(ArithmeticTokenType.Minus, "-"));
                    break;
                case '*':
                    tokens.Add(new Token(ArithmeticTokenType.Star, "*"));
                    break;
                case '/':
                    tokens.Add(new Token(ArithmeticTokenType.Slash, "/"));
                    break;
                case '!':
                    tokens.Add(Match('=') ? new Token(ComparisonTokenType.NotEqual, "!=") : new Token(LogicalTokenType.Bang, "!"));
                    break;
                case '=':
                    tokens.Add(Match('=') ? new Token(ComparisonTokenType.EqualEqual, "==") : new Token(OtherTokenType.Identifier, "="));
                    break;
                case '<':
                    tokens.Add(Match('=') ? new Token(ComparisonTokenType.LessEqual, "<=") : new Token(ComparisonTokenType.Less, "<"));
                    break;
                case '>':
                    tokens.Add(Match('=') ? new Token(ComparisonTokenType.GreaterEqual, ">=") : new Token(ComparisonTokenType.Greater, ">"));
                    break;
                case '&':
                    if (Match('&'))
                        tokens.Add(new Token(LogicalTokenType.And, "&&"));
                    else
                        throw new Exception("Unexpected character '&'");
                    break;
                case '|':
                    if (Match('|'))
                        tokens.Add(new Token(LogicalTokenType.Or, "||"));
                    else
                        throw new Exception("Unexpected character '|'");
                    break;
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    // Ignore whitespace characters
                    break;
                default:
                    if (char.IsDigit(c))
                        tokens.Add(ParseNumber());
                    else if (char.IsLetter(c))
                        tokens.Add(ParseIdentifier());
                    else
                        throw new Exception($"Unexpected character: {c}");
                    break;
            }
        }

        // End of file token
        tokens.Add(new Token(OtherTokenType.Eof, null));
        return tokens;
    }

    /// <summary>
    /// Parses a numeric literal (integer or floating-point) from the input.
    /// </summary>
    /// <returns>A token representing the parsed numeric value.</returns>
    Token ParseNumber()
    {
        var start = _currentIndex - 1;

        // Parse digits until the end of the number
        while (!IsAtEnd() && char.IsDigit(Peek()))
            Advance();

        // Handle possible decimal points for floating point numbers
        if (Peek() == '.')
        {
            Advance();// Consume the '.'

            // Parse digits after the decimal point
            while (!IsAtEnd() && char.IsDigit(Peek()))
                Advance();
        }

        var number = _input[start.._currentIndex];
        return new Token(LiteralTokenType.Number, double.Parse(number));
    }

    /// <summary>
    /// Parses an identifier (such as a variable or function name) from the input.
    /// </summary>
    /// <returns>A token representing the parsed identifier.</returns>
    Token ParseIdentifier()
    {
        var start = _currentIndex - 1;

        // Parse until the next non-letter or non-digit character
        while (!IsAtEnd() && char.IsLetterOrDigit(Peek()))
            Advance();

        var text = _input[start.._currentIndex];

        // Return boolean literals or identifier
        return text switch
        {
            "true" => new Token(LiteralTokenType.True, true),
            "false" => new Token(LiteralTokenType.False, false),
            _ => new Token(OtherTokenType.Identifier, text)
        };
    }

    /// <summary>
    /// Advances the current index and returns the character at the new index.
    /// </summary>
    /// <returns>The character at the current index.</returns>
    char Advance() => _input[_currentIndex++];

    /// <summary>
    /// Matches the current character with the expected character and advances the index if matched.
    /// </summary>
    /// <param name="expected">The character to match.</param>
    /// <returns>True if the current character matches the expected character, otherwise false.</returns>
    bool Match(char expected)
    {
        if (IsAtEnd() || _input[_currentIndex] != expected) return false;
        _currentIndex++;
        return true;
    }

    /// <summary>
    /// Returns the current character without advancing the index.
    /// </summary>
    /// <returns>The character at the current index.</returns>
    char Peek() => IsAtEnd() ? '\0' : _input[_currentIndex];

    /// <summary>
    /// Checks if the end of the input string has been reached.
    /// </summary>
    /// <returns>True if the end of the input is reached, otherwise false.</returns>
    bool IsAtEnd() => _currentIndex >= _input.Length;
}
