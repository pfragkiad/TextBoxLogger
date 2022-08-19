
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Runtime.Versioning;


namespace TextBoxLoggerLibrary;


[UnsupportedOSPlatform("browser")]
[ProviderAlias("TextBox")]
public class TextBoxLoggerProvider : ControlLoggerProvider<TextBox, TextBoxLogger>
{
    public TextBoxLoggerProvider(
     IConfiguration config,
     Control textBox) : base(config, textBox as TextBox) { }  
}