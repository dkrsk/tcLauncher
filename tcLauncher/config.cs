using DnKR.tcLauncher.tcUpdater;

namespace DnKR.tcLauncher
{
    internal static class config
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