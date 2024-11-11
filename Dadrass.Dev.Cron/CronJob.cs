namespace Dadrass.Dev.Cron;

using Models;
/// <summary>
/// Pattern layout 00:00:00 0 0 0 0 0
///                hh:mm:ss m s M h d
/// The first part is the exact hour that job will start at and other parts declare interval time from milliseconds to day
/// </summary>
/// <param name="pattern"></param>
public sealed class CronJob(string pattern) {
    TimePattern Pattern { get; } = TimePattern.Parse(pattern);
    readonly CancellationTokenSource _cancellationTokenSource = new();
    /// <summary>
    /// Each time that interval tick this event will be called
    /// First time it call on base time given in pattern
    /// </summary>
    public event EventHandler? Tick;
    
    /// <summary>
    /// Stop the job
    /// </summary>
    public void Stop() => _cancellationTokenSource.Cancel();
    /// <summary>
    /// Start the job
    /// </summary>
    public void Start() => new Task(Action, _cancellationTokenSource.Token).Start();
    async void Action()
    {
        try
        {
            await Task.Delay(Pattern.BaseWait, _cancellationTokenSource.Token);
            if (_cancellationTokenSource.Token.IsCancellationRequested) return;
                
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                await Task.Delay(Pattern.Millisecond, _cancellationTokenSource.Token);
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                await Task.Delay(Pattern.Second * 1000, _cancellationTokenSource.Token);
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                await Task.Delay(Pattern.Minute * 1000 * 60, _cancellationTokenSource.Token);
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                await Task.Delay(Pattern.Hour * 1000 * 60 * 60, _cancellationTokenSource.Token);
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                await Task.Delay(Pattern.Day * 1000 * 60 * 60 * 24, _cancellationTokenSource.Token);
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                OnTick();
            }
        }
        catch
        {
            //Ignored
        }
    }
    void OnTick()
    {
        Tick?.Invoke(this, EventArgs.Empty);
    }
}
