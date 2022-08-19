
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Runtime.Versioning;


namespace TextBoxLoggerLibrary;


[UnsupportedOSPlatform("browser")]
[ProviderAlias("Control")]
public class ControlLoggerProvider<TControl, TControlLogger> : ILoggerProvider 
    where TControlLogger:ControlLogger<TControl>
    where TControl:Control
{
    //private readonly IDisposable _onChangeToken;
    protected readonly IConfiguration _configuration;
    protected TControl _control;
    protected readonly ConcurrentDictionary<string, TControlLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    public ControlLoggerProvider(
        IConfiguration configuration,
        TControl control) =>
        (_configuration, _control) = (configuration, control);

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name =>
        (TControlLogger)Activator.CreateInstance(typeof(TControlLogger), name, _configuration, _control));
          //  new TControlLogger(name, _configuration, _control));

    public void Dispose()
    {
        _loggers.Clear();
    }
}