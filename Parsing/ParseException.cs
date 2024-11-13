namespace Dadrass.Dev.Cron.Parsing;

using System;

/// <summary>
/// Represents an error that occurs during parsing of an expression.
/// </summary>
public class ParseException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="ParseException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ParseException(string message) : base(message) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="ParseException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of this exception.</param>
    public ParseException(string message, Exception innerException) : base(message, innerException) {}
}
