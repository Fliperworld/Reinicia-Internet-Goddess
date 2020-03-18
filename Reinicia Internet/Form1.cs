using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace Reinicia_Internet
{
    public partial class Form1 : Form
    {
        private readonly KeyboardHook hook;
        private bool allowshowdisplay = false;
        private readonly bool ModoFirewall;

        public Form1()
        {
            InitializeComponent();

            ModoFirewall = WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);

            hook = new KeyboardHook();
            hook.RegisterHotKey(Reinicia_Internet.ModifierKeys.Win, Keys.F12);
            hook.KeyPressed += TeclaDetectada;
        }

        private async void TeclaDetectada(object sender, KeyPressedEventArgs e)
        {
            if (e.Modifier != Reinicia_Internet.ModifierKeys.Win)
                return;
            if (e.Key == Keys.F12)
            {
                Process process = new Process();
                if (!ModoFirewall)
                {
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "ipconfig";
                process.StartInfo.Arguments = "/release";
                process.Start();
                process.WaitForExit();

                await System.Threading.Tasks.Task.Delay(500);

                process = new Process();
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "ipconfig";
                process.StartInfo.Arguments = "/renew";
                process.Start();
                process.WaitForExit();
                return;
                }
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = "advfirewall firewall add rule name=\"Goddess porta 40021\" dir=out protocol=tcp action=block remoteport=40021 enable=yes profile=any";
                process.Start();
                process.WaitForExit();

                await System.Threading.Tasks.Task.Delay(3000);

                process = new Process();
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = "advfirewall firewall delete rule name=\"Goddess porta 40021\" dir=out";
                process.Start();
                process.WaitForExit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hook.Dispose();
        }

        private void notifyIcon1_Click(object sender, System.EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }

    }
}
