using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;
using WindowsFirewallHelper;
using WindowsFirewallHelper.FirewallAPIv2;
using WindowsFirewallHelper.FirewallAPIv2.Rules;
using System.Diagnostics;

namespace LagSwitch
{

    class Program
    {
        static bool isRunning = true;
        static string location;
        static string firewhash = System.Guid.NewGuid().ToString();

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Using Policy Hash " + firewhash);
            Console.WriteLine("Enter Path for Arma 3 : ");
            location = Console.ReadLine();
            Console.WriteLine("Location Set : " + location);
            Console.WriteLine("Lag Switch is now available - have fun xd");
            Start();
        }

        [STAThread]
        static void KB()
        {
            // Creating a new Process
            Process process = new Process();

            // Stop the process from opening a new window
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Setup executable and parameters
            process.StartInfo.FileName = "netsh.exe";

            while (isRunning)
            {

                Thread.Sleep(40);

                if ((Keyboard.GetKeyStates(Key.F1) & KeyStates.Down) > 0)
                {
                    process.StartInfo.Arguments = ("advfirewall firewall add rule name =\"" + firewhash + "\" dir=in action=block program=\"" + location + "\" enable=yes");
                    process.StartInfo.Arguments = ("advfirewall firewall add rule name =\"" + firewhash + "\" dir=out action=block program=\"" + location + "\" enable=yes");
                    process.Start();
                    process.Start();
                    Console.WriteLine("Blocked");
                }

                if ((Keyboard.GetKeyStates(Key.F2) & KeyStates.Down) > 0)
                {
                    process.StartInfo.Arguments = ("advfirewall firewall delete rule name=\"" + firewhash + "\" ");
                    process.Start();
                    Console.WriteLine("Unblocked");
                }
            }
        }
        static void Start()
        {
            Thread TH = new Thread(KB);
            TH.SetApartmentState(ApartmentState.STA);
            TH.Start();
        }
    }
}
