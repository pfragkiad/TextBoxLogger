using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBoxLoggerLibrary;

public interface ILogForm
{
    public Control LogControl { get; }

    public void Show();
}
