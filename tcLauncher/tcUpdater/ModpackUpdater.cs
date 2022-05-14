using System.Net;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;



namespace DnKR.tcLauncher.tcUpdater
{

    public delegate void StateChanged(string message, bool uiState);
    public static class ModpackUpdater
    {
        public static async Task UpdateModpack(UpdaterConfig config, StateChanged stateChanged, bool isChecking)
        {
            UpdateBuilder updater = new(config);
            string gamePath = config.path;

            string[] remoteNames;
            try
            {
                remoteNames = await updater.GetNamesAsync();
            }
            catch (WebException)
            {
                stateChanged("Failed to connect\nto the server", true);
                return;
            }

            string packageName = remoteNames[^1];
            short remoteVersion = Convert.ToInt16(packageName[0..^4]);
            short currentVersion;

            string infoPath = Path.Combine(gamePath, "info");

            stateChanged("Updating..", isChecking);

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
        public string path { get; }
        public string uri { get; }
        public string ftpPath { get; }
        public NetworkCredential credential { get; }

        public UpdaterConfig(string path, string uri, string ftpPath, NetworkCredential credential)
        {
            this.path = path;
            this.uri = uri;
            this.ftpPath = ftpPath;
            this.credential = credential;
        }
    }
}