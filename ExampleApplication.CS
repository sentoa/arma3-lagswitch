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

namespace ConsoleApp2
{

    class Program
    {
       static bool isRunning = true;
        [STAThread]
        static void Main(string[] args)
        {
            Recursive();
        }

        [STAThread]
        static void KB()
        {
            while (isRunning)
            {
                    var rule = new StandardRuleWin8("LS", @"C:\Program Files (x86)\Steam\steamapps\common\Arma 3\arma3.exe", FirewallAction.Block,
                        FirewallDirection.Outbound,
                        FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public)
                    {
                        Description =
                            "'My Port Rule' Allows Inbound traffic to my local Proxy Server from Wireless Adapters",
                        InterfaceTypes = FirewallInterfaceTypes.Wireless,
                        Protocol = FirewallProtocol.TCP
                    };

                Thread.Sleep(40);

                if ((Keyboard.GetKeyStates(Key.F1) & KeyStates.Down) > 0)
                {
                    Console.WriteLine("Blocked");
                    WindowsFirewallHelper.FirewallAPIv2.Firewall.Instance.Rules.Add(rule);
                }

                if ((Keyboard.GetKeyStates(Key.F2) & KeyStates.Down) > 0)
                {
                    Console.WriteLine("Unblocked");
                    FirewallManager.Instance.Rules.Remove(rule);
                    WindowsFirewallHelper.FirewallAPIv2.Firewall.Instance.Rules.Remove(rule);
                }
            }
        }
        static void Recursive()
        {
            Thread TH = new Thread(KB);
            TH.SetApartmentState(ApartmentState.STA);
            TH.Start();
        }
    }
}
