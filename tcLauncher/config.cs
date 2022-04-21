using DnKR.mineUpdater;

namespace DnKR.tcLauncher
{
    internal class config
    {
        public static UpdaterConfig GetUpdConfig()
        {
            return new UpdaterConfig(
                    "gameDir",
                    "server uri",
                    "ftp path",
                    new System.Net.NetworkCredential("user", "pass")
                );
        }
    }
}
//Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.tclaucner",
//                    "ftp://82.208.78.206/",
//                    "mainUPD/",
//                    new System.Net.NetworkCredential("almakael", "user1234")