using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using june19;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
[assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
  public class Script
  {
    public Script()
    {
    }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(ScriptContext context, System.Windows.Window window/*, ScriptEnvironment environment*/)
        {
            // TODO : Add here the code that is called when the script is launched from Eclipse.
            //Patient patient = context.Patient;
            //patient.BeginModifications();

            Window wi = new Window();
            wi.Height = 700;
            wi.Width = 1150;
            //Logic logic = new Logic(context.PlanSetup);
            var ui = new UI(context);
            wi.Content=ui;
            wi.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wi.ShowDialog();
            //wi.SizeToContent = SizeToContent.WidthAndHeight;

        }
  }
}
