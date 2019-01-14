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
                    var inbound = new StandardRuleWin8("LS", 2302, FirewallAction.Block,
                        FirewallDirection.Inbound,
                        FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public)
                    {
                        Description =
                            "'LS' LS - Inbound",
                        InterfaceTypes = FirewallInterfaceTypes.Wireless,
                        Protocol = FirewallProtocol.TCP
                    };

                var outbound = new StandardRuleWin8("LS", 2302, FirewallAction.Block,
                       FirewallDirection.Outbound,
                       FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public)
                {
                    Description =
                           "'LS' LS - Outbound",
                    InterfaceTypes = FirewallInterfaceTypes.Wireless,
                    Protocol = FirewallProtocol.TCP
                };

                Thread.Sleep(40);

                if ((Keyboard.GetKeyStates(Key.F1) & KeyStates.Down) > 0)
                {
                    Console.WriteLine("Blocked");
                    WindowsFirewallHelper.FirewallAPIv2.Firewall.Instance.Rules.Add(inbound);
                    WindowsFirewallHelper.FirewallAPIv2.Firewall.Instance.Rules.Add(outbound);
                }

                if ((Keyboard.GetKeyStates(Key.F2) & KeyStates.Down) > 0)
                {
                    Console.WriteLine("Unblocked");
                    WindowsFirewallHelper.FirewallAPIv2.Firewall.Instance.Rules.Remove(outbound);
                    WindowsFirewallHelper.FirewallAPIv2.Firewall.Instance.Rules.Remove(inbound);
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
