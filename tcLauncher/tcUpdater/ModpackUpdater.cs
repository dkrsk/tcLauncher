using System.Net;
using System.Diagnostics;
using System;
using System.IO;



namespace DnKR.tcLauncher.tcUpdater
{

    public delegate void StateChanged(string message, bool uiState);
    public static class ModpackUpdater
    {
        public static async void UpdateModpack(UpdaterConfig config, StateChanged stateChanged, bool isChecking = false)
        {
            UpdateBuilder updater = new(config);
            string gamePath = config.path;

            string[] remoteNames = await updater.GetNamesAsync();
            string packageName = remoteNames[^1];
            short remoteVersion = Convert.ToInt16(packageName[0..^4]);
            short currentVersion;

            string infoPath = Path.Combine(gamePath, "info");

            stateChanged("Updating..", false);

            try
            {
                using (StreamReader sr = new(infoPath))
                {
                    currentVersion = Convert.ToInt16(sr.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                currentVersion = 0;
            }

            if(remoteVersion > currentVersion)
            {
                if (isChecking)
                {
                    stateChanged("A new version was found!", true);
                    return;
                }

                await updater.DownloadFileAsync(packageName);

                await updater.ExtractFileAsync(packageName);

                if (File.Exists(gamePath + "\\tmpUpdateLauncher")) // not stable
                {
                    Process currentProc = Process.GetCurrentProcess();
                    Process.Start($"{gamePath}\\tcUpdater.exe", $"{gamePath}\\tmpUpdateLauncher {currentProc.MainModule.FileName} {currentProc.Id} {packageName}");
                }

                using (StreamWriter fw = new StreamWriter(infoPath, false))
                {
                    fw.Write(remoteVersion);
                }

                stateChanged($"Success!\nUpdated to build{remoteVersion}", true);
            }
            else
            {
                stateChanged("You have the latest version!", true);
            }
        }
    }

    public class UpdaterConfig
    {
        public readonly string path;
        public readonly string uri;
        public readonly string ftpPath;
        public readonly NetworkCredential credential;

        public UpdaterConfig(string path, string uri, string ftpPath, NetworkCredential credential)
        {
            this.path = path;
            this.uri = uri;
            this.ftpPath = ftpPath;
            this.credential = credential;
        }
    }
}