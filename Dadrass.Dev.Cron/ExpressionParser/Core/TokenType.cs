namespace Dadrass.Dev.Cron.ExpressionParser.Core;

/// <summary>
/// Defines token types for arithmetic operators.
/// </summary>
public enum ArithmeticTokenType {
    /// <summary>Represents the "+" operator.</summary>
    Plus,

    /// <summary>Represents the "-" operator.</summary>
    Minus,

    /// <summary>Represents the "*" operator.</summary>
    Star,

    /// <summary>Represents the "/" operator.</summary>
    Slash
}

/// <summary>
/// Defines token types for logical operators.
/// </summary>
public enum LogicalTokenType {
    /// <summary>Represents the "&&" operator.</summary>
    And,

    /// <summary>Represents the "||" operator.</summary>
    Or,

    /// <summary>Represents the "!" operator.</summary>
    Bang
}

/// <summary>
/// Defines token types for comparison operators.
/// </summary>
public enum ComparisonTokenType {
    /// <summary>Represents the "<" operator.</summary>
    Less,

    /// <summary>Represents the "<=" operator.</summary>
    LessEqual,

    /// <summary>Represents the ">" operator.</summary>
    Greater,

    /// <summary>Represents the ">=" operator.</summary>
    GreaterEqual,

    /// <summary>Represents the "==" operator.</summary>
    EqualEqual,

    /// <summary>Represents the "!=" operator.</summary>
    NotEqual
}

/// <summary>
/// Defines token types for literals (numbers, booleans, etc.).
/// </summary>
public enum LiteralTokenType {
    /// <summary>Represents the boolean literal "true".</summary>
    True,

    /// <summary>Represents the boolean literal "false".</summary>
    False,

    /// <summary>Represents a numeric literal (e.g., 42, 3.14).</summary>
    Number,

    /// <summary>Represents a string literal (e.g., "hello").</summary>
    String
}

/// <summary>
/// Defines other token types such as parentheses and identifiers.
/// </summary>
public enum OtherTokenType {
    /// <summary>Represents a variable or function name identifier.</summary>
    Identifier,

    /// <summary>Represents the left parenthesis "(". </summary>
    LeftParen,

    /// <summary>Represents the right parenthesis ")".</summary>
    RightParen,

    /// <summary>Represents the end of the file (EOF) token.</summary>
    Eof
}
