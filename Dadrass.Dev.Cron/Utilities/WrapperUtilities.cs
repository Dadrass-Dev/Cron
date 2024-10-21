namespace Dadrass.Dev.Cron.Utilities;

public static class WrapperUtilities {
    public static TOut _<TIn, TOut>(this TIn obj, Func<TIn, TOut> callback) => callback(obj);
}
