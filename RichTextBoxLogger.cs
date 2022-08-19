using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace TextBoxLoggerLibrary;

//https://stackoverflow.com/questions/57509951/use-windows-forms-in-a-net-core-class-library-net-core-control-library


//TODO: Add custom colors via options
public class RichTextBoxLogger : ControlLogger<RichTextBox>
{
    //private readonly Func<ColorConsoleLoggerConfiguration> _getCurrentConfig;

    //https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
    public RichTextBoxLogger(
        string name,
        IConfiguration configuration,
        RichTextBox textbox) : base(name, configuration, textbox) { }

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
        Color color = logLevel switch //can be customizer
        {
            LogLevel.Debug => Color.DarkGray,
            LogLevel.Information => Color.DarkGreen,
            LogLevel.Warning => Color.Magenta,
            LogLevel.Error => Color.Red,
            LogLevel.Critical => Color.DarkRed,
            _ => Color.Black
        };

        DoSafe(() =>
        {
            _control.SelectionStart = _control.TextLength;
            _control.SelectionColor = color;
            _control.AppendText(text);
            _control.ScrollToCaret();
        });
    }
}


