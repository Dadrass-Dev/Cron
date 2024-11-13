namespace Dadrass.Dev.Cron.Lexing;

/// <summary>
/// Defines the various types of tokens recognized in expressions.
/// </summary>
public enum TokenType {
    /// <summary>Represents a numeric value, e.g., 42.</summary>
    Number,

    /// <summary>Represents an identifier, such as a field name, e.g., "age".</summary>
    Identifier,

    /// <summary>Represents a string literal, e.g., "active".</summary>
    String,

    /// <summary>Represents the addition operator, +.</summary>
    Plus,

    /// <summary>Represents the subtraction operator, -.</summary>
    Minus,

    /// <summary>Represents the multiplication operator, *.</summary>
    Multiply,

    /// <summary>Represents the division operator, /.</summary>
    Divide,

    /// <summary>Represents the greater-than operator, >.</summary>
    GreaterThan,

    /// <summary>Represents the less-than operator, <.</summary>
    LessThan,

    /// <summary>Represents the greater-than-or-equal-to operator, >=.</summary>
    GreaterThanOrEqual,

    /// <summary>Represents the less-than-or-equal-to operator, <=.</summary>
    LessThanOrEqual,

    /// <summary>Represents the equality operator, ==.</summary>
    Equal,

    /// <summary>Represents the inequality operator, !=.</summary>
    NotEqual,

    /// <summary>Represents the logical AND operator, &&.</summary>
    And,

    /// <summary>Represents the logical OR operator, ||.</summary>
    Or,

    /// <summary>Represents the left parenthesis, (.</summary>
    LeftParenthesis,

    /// <summary>Represents the right parenthesis, ).</summary>
    RightParenthesis,

    /// <summary>Indicates the end of the expression.</summary>
    EndOfExpression
}
