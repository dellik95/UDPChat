using Microsoft.Win32;

namespace DarkBlue
{
    public  class autorun
    {
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        public void SetAutoRun()
        {

            rkApp.SetValue("MyApp", System.Windows.Forms.Application.ExecutablePath.ToString());
        }
        public void DeleteAutorun()
        {
            rkApp.DeleteValue("MyApp", false);
        }
    }
}
