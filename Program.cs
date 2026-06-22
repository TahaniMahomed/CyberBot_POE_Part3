/* S!--CODE ATTRIBUTION
TITLE: Microsoft Learn: Application.Run Method
AUTHOR: Microsoft WPF Team
DATE: 13 May 2026
VERSION: WPF Documentation
AVAILABLE: https://learn.microsoft.com/en-us/dotnet/api/system.windows.application.run
*/

/* S!--CODE ATTRIBUTION
TITLE: WPF-Tutorial: Manual Main Method
AUTHOR: WPF-Tutorial.com
DATE: 13 May 2026
VERSION: Tutorial Publication
AVAILABLE: https://www.wpftutorial.net/AppStartup.html
*/

/* S!--CODE ATTRIBUTION
TITLE: Microsoft Learn: STAThreadAttribute Class
AUTHOR: Microsoft .NET Team
DATE: 13 May 2026
VERSION: .NET Documentation
AVAILABLE: https://learn.microsoft.com/en-us/dotnet/api/system.stathreadattribute
*/

/* S!--CODE ATTRIBUTION
TITLE: Stack Overflow: Why is STAThread required in WPF?
AUTHOR: Stack Overflow Contributors
DATE: 13 May 2026
VERSION: Community Q&A
AVAILABLE: https://stackoverflow.com/questions/518701/why-is-stathread-required
*/

using System;

namespace CyberBot_POE
{
    /// <summary>
    /// The static entry point for the application. 
    /// This class handles the initial startup logic and transitions the execution to the WPF framework.
    /// </summary>
    public class Program
    {
        // The STAThread attribute ensures the COM threading model for the application is Single-Threaded Apartment, 
        // which is a strict requirement for running Windows Presentation Foundation (WPF) UI threads.
        [STAThread] 
        public static void Main()
        {
            // Initializes the global WPF Application instance which manages the lifecycle of the software.
            var app = new System.Windows.Application();

            // Launches the application and sets 'MainWindow' as the primary interface for the user.
            // Execution will remain within this 'Run' method until the window is closed.
            app.Run(new MainWindow());
        }
    }
}