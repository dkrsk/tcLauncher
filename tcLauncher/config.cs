using System;
using DnKR.tcLauncher.tcUpdater;

namespace DnKR.tcLauncher
{
    internal static class config
    {
        public static UpdaterConfig GetUpdConfig()
        {
            return new UpdaterConfig(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.tclaucner", // game directory
                    "ftp://127.0.0.1", // ftp server ip
                    "mainUPD/", // modpack direcotry on the server
                    new System.Net.NetworkCredential("user", "pass") // user and password for ftp
                );
        }
    }
}