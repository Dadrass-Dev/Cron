namespace Dadrass.Dev.Cron;

using System;
using System.Threading;
using System.Threading.Tasks;
using Events;
using Models;
using Dadrass.Dev.Expression;
using Expression.Core.Utilities;

/// <summary>
/// Represents a customizable cron job that runs tasks on a specified pattern-based schedule.
/// </summary>
/// <remarks>
/// Pattern layout: "hh:mm:ss m s M h d" where:
/// - hh:mm:ss - The initial start time for the job
/// - m - Interval in milliseconds
/// - s - Interval in seconds
/// - M - Interval in minutes
/// - h - Interval in hours
/// - d - Interval in days
/// 
/// Example: "00:00:00 0 5 0 0 0" runs the job every 5 seconds, starting at midnight.
/// </remarks>
public sealed class CronJob {
    readonly CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// Function registry for custom functions in expression
    /// </summary>
    public readonly Dictionary<string, Func<object?[]?, object>> CustomFunctionRegistry = [];

    /// <summary>
    /// Gets or sets the current tick sequence number.
    /// </summary>
    /// <remarks>
    /// This counter increments each time the cron job triggers the <see cref="Tick"/> event,
    /// providing a reliable sequence number that can be used by event handlers to track execution cycles.
    /// </remarks>
    public int Sequence { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CronJob"/> class with the specified pattern.
    /// </summary>
    /// <param name="pattern">
    /// A string representing the cron schedule pattern.
    /// </param>
    /// <param name="expression">
    /// An expression to maximize flexibility of cron ticks conditions
    /// </param>
    /// <param name="datasource">
    /// Data source of provided params in the Expression
    /// </param>
    public CronJob(string pattern, string expression = "true==true", Dictionary<string, object>? datasource = null)
    {
        Datasource = datasource;
        ExpressionString = expression;
        Pattern = TimePattern.TryParse(pattern);
        foreach (var keyValuePair in CustomFunctionRegistry)
        {
            ExpressionUtilities.CustomFunctionRegistry.TryAdd(keyValuePair.Key, keyValuePair.Value);
        }
    }

    /// <summary>
    /// Gets the parsed time pattern based on the provided pattern string.
    /// </summary>
    TimePattern Pattern { get; }

    string ExpressionString { get; }
    Dictionary<string, object>? Datasource { get; }

    /// <summary>
    /// Occurs each time the interval defined in the pattern elapses.
    /// </summary>
    /// <remarks>
    /// The first tick occurs at the base time specified in the pattern,
    /// with subsequent ticks happening at intervals defined by the pattern.
    /// </remarks>
    public event TickEventHandler? Tick;

    /// <summary>
    /// Starts the cron job according to the specified time pattern.
    /// </summary>
    /// <remarks>
    /// The job will run until <see cref="Stop"/> is called or the application shuts down.
    /// </remarks>
    public void Start() => new Task(RunTaskAsync, _cancellationTokenSource.Token).Start();

    /// <summary>
    /// Stops the cron job, halting all future executions.
    /// </summary>
    public void Stop() => _cancellationTokenSource.Cancel();

    /// <summary>
    /// Executes the cron job task at intervals based on the parsed <see cref="Pattern"/>.
    /// </summary>
    /// <remarks>
    /// This method runs asynchronously and will halt execution if a cancellation is requested.
    /// </remarks>
    async void RunTaskAsync()
    {
        try
        {
            // Wait until the initial start time defined by the pattern
            if (await WaitOrCancelAsync(delay: Pattern.BaseWait)) return;
            if (Expression.Evaluate<bool>(ExpressionString, Datasource))
                TriggerTickEvent();

            // Run the task in intervals defined by the pattern until stopped
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (await WaitOrCancelAsync(delay: Pattern.Millisecond)) return;
                if (await WaitOrCancelAsync(delay: Pattern.Second * 1000)) return;
                if (await WaitOrCancelAsync(delay: Pattern.Minute * 1000 * 60)) return;
                if (await WaitOrCancelAsync(delay: Pattern.Hour * 1000 * 60 * 60)) return;
                if (await WaitOrCancelAsync(delay: Pattern.Day * 1000 * 60 * 60 * 24)) return;

                if (Expression.Evaluate<bool>(ExpressionString, Datasource))
                    TriggerTickEvent();
                Sequence++;// Increment the sequence count after each tick event
            }
        }
        catch (OperationCanceledException)
        {
            // Expected exception on cancellation, no action required
        }
        catch (Exception ex)
        {
            // Log or handle any unexpected exceptions as needed
            Console.WriteLine($"CronJob error: {ex.Message}");
        }
    }

    async Task<bool> WaitOrCancelAsync(int delay)
    {
        await Task.Delay(delay, _cancellationTokenSource.Token);
        return _cancellationTokenSource.Token.IsCancellationRequested;
    }

    /// <summary>
    /// Invokes the <see cref="Tick"/> event to notify listeners of an interval tick.
    /// </summary>
    /// <remarks>
    /// Each time this method is called, the <see cref="Sequence"/> number is incremented to indicate 
    /// a new tick cycle has completed, providing a sequential count of interval events.
    /// </remarks>
    void TriggerTickEvent() => Tick?.Invoke(this, new TickEventArgs(Pattern.ToString(), Sequence));
}
