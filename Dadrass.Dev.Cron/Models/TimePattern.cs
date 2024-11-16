namespace Dadrass.Dev.Cron.Models;

using Utilities;

/// <summary>
/// Represents a parsed time pattern for scheduling cron jobs with customizable intervals.
/// </summary>
/// <remarks>
/// The <see cref="TimePattern"/> class defines specific intervals, including milliseconds, seconds, minutes,
/// hours, and days, based on a pattern string provided at initialization.
/// </remarks>
class TimePattern(int baseWait, int millisecond, int second, int minute, int hour, int day) {
    /// <summary>
    /// Gets the initial wait time in milliseconds until the cron job should first execute.
    /// </summary>
    public int BaseWait { get; } = baseWait;

    /// <summary>
    /// Gets or sets the interval in milliseconds.
    /// </summary>
    public int Millisecond { get; set; } = millisecond;

    /// <summary>
    /// Gets or sets the interval in seconds.
    /// </summary>
    public int Second { get; set; } = second;

    /// <summary>
    /// Gets or sets the interval in minutes.
    /// </summary>
    public int Minute { get; set; } = minute;

    /// <summary>
    /// Gets or sets the interval in hours.
    /// </summary>
    public int Hour { get; set; } = hour;

    /// <summary>
    /// Gets or sets the interval in days.
    /// </summary>
    public int Day { get; set; } = day;

    /// <summary>
    /// Translates a time interval part from a string format to an integer, converting "*" to 1.
    /// </summary>
    /// <param name="pattern">The string representation of the time interval part.</param>
    /// <returns>An integer representing the time interval.</returns>
    /// <exception cref="FormatException">Thrown if the pattern cannot be parsed as an integer.</exception>
    static int Translate(string pattern) => pattern switch
    {
        "*" => 1,
        _ => int.Parse(pattern)
    };

    /// <summary>
    /// Parses a pattern string into a <see cref="TimePattern"/> object, calculating the base wait time and intervals.
    /// </summary>
    /// <param name="pattern">A string representing the cron-like schedule pattern (e.g., "00:00:00 0 5 0 0 0").</param>
    /// <returns>A <see cref="TimePattern"/> object that defines the interval timings for the job.</returns>
    /// <exception cref="FormatException">Thrown if the pattern is invalid or cannot be parsed.</exception>
    public static TimePattern TryParse(string pattern)
    {
        try
        {
            // Split the pattern string into time components
            var split = pattern.Split(' ');

            // Process and convert each time component
            return split
                ._(parts => parts.Skip(1).Select(Translate))
                .ToArray()
                ._(parts => {
                    // Calculate the sum of intervals in milliseconds
                    var sum = parts[0] + parts[1] * 1000 + parts[2] * 1000 * 60 + parts[3] * 1000 * 60 * 60 + parts[4] * 1000 * 60 * 60 * 24;

                    // Calculate the base wait time as the remaining time until the next start based on the current time
                    var baseWait = DateTime.Now.Subtract(DateTime.Parse(split[0]))
                        ._(difference => difference.TotalMilliseconds)
                        ._(millisecondsDifference => millisecondsDifference % sum)
                        ._(remaining => remaining < 0 ? remaining * -1 : remaining);

                    return new TimePattern((int)baseWait, parts[0], parts[1], parts[2], parts[3], parts[4]);
                });
        }
        catch (Exception ex)
        {
            throw new FormatException($"Invalid pattern: {pattern}", ex);
        }
    }

    /// <summary>
    /// Returns a string representation of the time pattern intervals.
    /// </summary>
    /// <returns>A string containing the interval values for milliseconds, seconds, minutes, hours, and days.</returns>
    override public string ToString() => $"{Millisecond} {Second} {Minute} {Hour} {Day}";
}
