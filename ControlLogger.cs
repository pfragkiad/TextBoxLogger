using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBoxLoggerLibrary;

public abstract class ControlLogger<TControl> : ILogger where TControl : Control
{
    protected readonly string _name;
    protected readonly IConfiguration _configuration;
    protected readonly TControl _control;

    //private readonly Func<ColorConsoleLoggerConfiguration> _getCurrentConfig;

    //https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
    public ControlLogger(
        string name,
        IConfiguration configuration,
        TControl textbox
        ) => (_name, _configuration, _control) = (name, configuration, textbox);

    public IDisposable BeginScope<TState>(TState state) => default!;

    //https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
    public virtual bool IsEnabled(LogLevel logLevel)
    {
        //Trace, Debug, Information, Warning, Error, Critical, None
        LogLevel minLevel = Enum.Parse<LogLevel>(_configuration["Logging:LogLevel:Default"]);
        return logLevel >= minLevel;
    }

    public abstract void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter);


    public void DoSafe(Control control, Action action)
    {
        if (control.InvokeRequired)
            control.Invoke(() => action());
        else
            action();
    }

    public void DoSafe(Action action) =>
        DoSafe(_control, action);
}

