namespace Dadrass.Dev.Cron;

using Models;

public sealed class CronJob(string pattern) {
    TimePattern Pattern { get; } = TimePattern.Parse(pattern);
    readonly CancellationTokenSource _cancellationTokenSource = new();

    public event EventHandler? Tick;
    public void Stop() => _cancellationTokenSource.Cancel();
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
