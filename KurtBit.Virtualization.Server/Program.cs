/*
 *  TODO(PPavlov):
 *      Add Auditing feature - sqlite or MSSQL
 *      Add Logging feature - file
 *      Refactor Commands, Create ManagmentServiceClass
 */

namespace KurtBit.Virtualization.Server
{
    using Microsoft.Owin.Hosting;
    using System;
    using System.Security.Principal;

    public class Program
    {
        static void Main(string[] args)
        {
            const string PORT = "8080";
            const string ADDRESS = "localhost";

            if(!IsRunAsAdministrator())
            {
                const int NOT_SUFFICIENT_PRIVILEGES_EXIT_CODE = -1;

                Console.WriteLine("This application requires elevated credentials in order to operate correctly!");
                Console.WriteLine("To correctly run this application, please run as administrator!");

                Environment.Exit(NOT_SUFFICIENT_PRIVILEGES_EXIT_CODE);
            }

            using (WebApp.Start<Startup>($"http://{ADDRESS}:{PORT}"))
            {               
                Console.WriteLine($"Web Server is running at address: {ADDRESS}, on port: {PORT}");

                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
            }
        }

        private static bool IsRunAsAdministrator()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
