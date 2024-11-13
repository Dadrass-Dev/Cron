namespace Dadrass.Dev.Cron.Lexing;

/// <summary>
/// Represents a token identified in the expression, containing its type and value.
/// </summary>
public class Token {
    /// <summary>
    /// Gets the type of the token.
    /// </summary>
    public TokenType Type { get; }

    /// <summary>
    /// Gets the value of the token, if applicable.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class.
    /// </summary>
    /// <param name="type">The type of the token.</param>
    /// <param name="value">The value of the token, if any.</param>
    public Token(TokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }
}
