namespace Dadrass.Dev.Cron.Models;

using Utilities;

class TimePattern(int baseWait, int millisecond, int second, int minute, int hour, int day) {
    public int BaseWait { get; } = baseWait;
    public int Millisecond { get; set; } = millisecond;
    public int Second { get; set; } = second;
    public int Minute { get; set; } = minute;
    public int Hour { get; set; } = hour;
    public int Day { get; set; } = day;

    static int Translate(string pattern) => pattern switch
    {
        "*" => 1,
        _ => int.Parse(pattern)
    };
    public static TimePattern Parse(string pattern)
    {
        var split = pattern.Split(' ');
        return split._(parts => parts.Skip(1).Select(Translate))
            .ToArray()
            ._(parts => {
                var sum = parts[0] + parts[1] * 1000 + parts[2] * 1000 * 60 + parts[3] * 1000 * 60 * 60 + parts[4] * 1000 * 60 * 60 * 24;

                var baseWait = DateTime.Now.Subtract(DateTime.Parse(split[0]))
                    ._(difference => difference.TotalMilliseconds)
                    ._(millisecondsDifference => millisecondsDifference % sum)
                    ._(remaining => remaining < 0 ? remaining * -1 : remaining);

                return new TimePattern((int)baseWait, parts[0], parts[1], parts[2], parts[3], parts[4]);
            });
    }
    override public string ToString() => $"{Millisecond} {Second} {Minute} {Hour} {Day}";
}
