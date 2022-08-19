using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace TextBoxLoggerLibrary;

//https://stackoverflow.com/questions/57509951/use-windows-forms-in-a-net-core-class-library-net-core-control-library

public class TextBoxLogger : ControlLogger<TextBox> 
{
    //private readonly Func<ColorConsoleLoggerConfiguration> _getCurrentConfig;

    //https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
    public TextBoxLogger(
        string name,
        IConfiguration configuration,
        TextBox textbox
        ) : base(name, configuration, textbox) { }

    public override void Log<TState>(
         LogLevel logLevel,
         EventId eventId,
         TState state,
         Exception? exception,
         Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        string text =
            !string.IsNullOrWhiteSpace(_name) ?
                $"{DateTime.Now: yyyy-MM-dd HH:mm:ss} [{logLevel}]: ({_name}) {formatter(state, exception)}\r\n" :
                $"{DateTime.Now: yyyy-MM-dd HH:mm:ss} [{logLevel}]: {formatter(state, exception)}\r\n";

        //watch for cross-thread operations
        DoSafe(() => _control.AppendText(text));
    }
}


