# TextBoxLogger

# About

Implements the ILogger interface with Windows Forms controls in .NET Core. The Library is a great example to show the use of DI in Windows Forms projects!
Dependency Inversion/Injection (DI) is a great way to improve the application operations in terms of testing, maintenance and reusability.

*MORE EXAMPLES/DOCUMENTATION TO FOLLOW - STAY TUNED!*

# How to use

A Form that implements the `ILogForm` interface can act as an `ILogger`!. The `ILogForm` interface is quite simple:

```cs
namespace TextBoxLoggerLibrary;

public interface ILogForm
{
    public Control LogControl { get; }

    public void Show();
}
```

Let's assume that we have a `Form` called `frmLog`, which implements this interface. We assume, that a `RichTextBox` (or a `TextBox`) are typical controls that can show logger messages. Note that because Dependency Injection is supported, we include an `IConfiguration` argument in the constructor, which can be used to retrieve settings from a local `appsettings.json` file.

```cs
namespace UcareWin;

public partial class frmLog : Form, ILogForm
{
    private readonly IConfiguration _configuration;

    public frmLog(IConfiguration configuration)
    {
        InitializeComponent();
        _configuration = configuration;
    }

    //--------------------------------------------------------
    //implemented function for the ILogForm
    //this is the only thing that we need to do within the form!
    public Control LogControl => richTextBoxLog // or txtLog;
    //--------------------------------------------------------

    private void frmLog_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        //txtLog.Clear();
        richTextBoxLog.Clear();
    }
}
```

We still need to configure the host services, in order to register the ILogForm. We need to change the `Main` entry function, in order to enable Dependency Injection as shown below:

```cs
//be sure that you have installed the Dependency Injection Nuget packages
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UcareWin;

internal static class Program
{

    static IHost? host;

    public static IServiceProvider Provider => host.Services;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var builder = Host
           .CreateDefaultBuilder()
           .ConfigureServices((context, services) =>
           {
               services
                   .AddSingleton<frmMain>() //this is the main form, which we assume that is called frmMain
                   .AddLogForm<frmLog>()
                   ;

           })
            //we can configure TextBox or RichTextBox logging by using one the following calls.
            //the non-commented code is used in our case
            //.ConfigureTextBoxLogging()
            .ConfigureRichTextBoxLogging()
         ;

        host = builder.Build();

        var form = host.Services.GetService<frmMain>();
        Application.Run(form);
    }
}

```

Within the main form (`frmMain`), a sample code to store/show the `logForm` instance is shown below:

```cs
ILogForm logForm;
protected override void OnLoad(EventArgs e)
{
    base.OnLoad(e);

    InitializeControls();
    logForm = Program.Provider.GetRequiredService<ILogForm>();
    logForm.Show();
}
```

That's it! Any logging message in our classes is shown in the registered `RichTextBox`/`TextBox` of our choice!


The `frmLog` example, assumes that a `RichTextBox` named `richTextBoxLog` is used. For completeness, the `frmLog.Designer.cs` code is given below:

```cs
namespace UcareWin
{
    partial class frmLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(490, 415);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(255, 46);
            this.txtLog.TabIndex = 4;
            this.txtLog.Visible = false;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(12, 415);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLog.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(776, 397);
            this.richTextBoxLog.TabIndex = 6;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.WordWrap = false;
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtLog);
            this.Name = "frmLog";
            this.Text = "Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtLog;
        private Button btnClear;
        private RichTextBox richTextBoxLog;
    }
}
```
