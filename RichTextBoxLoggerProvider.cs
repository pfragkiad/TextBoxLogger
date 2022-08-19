
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Runtime.Versioning;


namespace TextBoxLoggerLibrary;


[UnsupportedOSPlatform("browser")]
[ProviderAlias("RichTextBox")]
public class RichTextBoxLoggerProvider : ControlLoggerProvider<RichTextBox, RichTextBoxLogger>
{
    public RichTextBoxLoggerProvider(
     IConfiguration config,
     Control textBox) : base(config, textBox as RichTextBox) { }  
}