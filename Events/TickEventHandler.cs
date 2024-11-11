namespace Dadrass.Dev.Cron.Events;

/// <summary>
/// Represents the method that will handle the Tick event of a <see cref="CronJob"/>.
/// </summary>
/// <param name="sender">The source of the event, typically the <see cref="CronJob"/> instance.</param>
/// <param name="e">A <see cref="TickEventArgs"/> containing event data.</param>
public delegate void TickEventHandler(object sender, TickEventArgs e);

/// <summary>
/// Provides data for the <see cref="CronJob.Tick"/> event, including the pattern and sequence number.
/// </summary>
/// <param name="pattern">The pattern string used to initialize the cron job schedule.</param>
/// <param name="sequence">The sequence number of the current tick since the job started.</param>
public class TickEventArgs(string pattern, int sequence) : EventArgs {
    /// <summary>
    /// Gets or sets the pattern string used to initialize the cron job schedule.
    /// </summary>
    public string Pattern { get; set; } = pattern;

    /// <summary>
    /// Gets or sets the sequence number of the tick, which increments with each job execution.
    /// </summary>
    public int Sequence { get; set; } = sequence;
}
