namespace Dadrass.Dev.Cron.Utilities;

/// <summary>
/// Provides extension methods to enable functional-style transformations.
/// </summary>
static class WrapperUtilities {
    /// <summary>
    /// Applies a transformation function to an input object and returns the result.
    /// </summary>
    /// <typeparam name="TIn">The type of the input object.</typeparam>
    /// <typeparam name="TOut">The type of the result after applying the transformation.</typeparam>
    /// <param name="obj">The input object to be transformed.</param>
    /// <param name="callback">A function that defines the transformation to be applied to <paramref name="obj"/>.</param>
    /// <returns>The result of applying <paramref name="callback"/> to <paramref name="obj"/>.</returns>
    /// <example>
    /// Usage:
    /// <code>
    /// var result = someObject._(obj => obj.SomeTransformation());
    /// </code>
    /// </example>
    public static TOut _<TIn, TOut>(this TIn obj, Func<TIn, TOut> callback) => callback(obj);
}
