using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TextBoxLoggerLibrary;

public static class TextBoxLoggerProviderExtensions
{
    public static IServiceCollection AddLogForm<TForm>(this IServiceCollection services) where TForm : class, ILogForm
    {
        return
               services.AddSingleton<ILogForm, TForm>()
               //the log form should not be used in the Application.Run() call
               .AddSingleton(provider => provider.GetService<ILogForm>().LogControl);
    }
     
    public static ILoggingBuilder AddTextBoxLogger(
        this ILoggingBuilder builder, HostBuilderContext context)
    {
        //builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, TextBoxLoggerProvider>());

        //LoggerProviderOptions.RegisterProviderOptions  <IConfiguration, TextBoxLoggerProvider>(builder.Services);

        return builder;
    }


    public static IHostBuilder ConfigureTextBoxLogging(this IHostBuilder builder)
    {
        return builder.ConfigureLogging((HostBuilderContext context, ILoggingBuilder builder) =>
                 // builder.ClearProviders();
                 builder.AddTextBoxLogger(context));
    }

    //----------

    public static ILoggingBuilder AddRichTextBoxLogger(
    this ILoggingBuilder builder, HostBuilderContext context)
    {
        //builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, RichTextBoxLoggerProvider>());

        //LoggerProviderOptions.RegisterProviderOptions  <IConfiguration, TextBoxLoggerProvider>(builder.Services);

        return builder;
    }

    public static IHostBuilder ConfigureRichTextBoxLogging(this IHostBuilder builder)
    {
        return builder.ConfigureLogging((HostBuilderContext context, ILoggingBuilder builder) =>
                 // builder.ClearProviders();
                 builder.AddRichTextBoxLogger(context));
    }




}
