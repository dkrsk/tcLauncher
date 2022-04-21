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