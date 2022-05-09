using System.Diagnostics;
using System.ComponentModel;

using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;

using DnKR.tcLauncher.tcUpdater;

namespace DnKR.tcLauncher
{
    public class MGame
    {
        public CMLauncher launcher;
        public MLaunchOption launchOption;

        public MinecraftPath GamePath { get; } = new MinecraftPath(config.GetUpdConfig().path);
        public string? GameVersion { get; set; }

        public event DataReceivedEventHandler OutputRecieved;
        public event EventHandler ProcessExited;

        public event ProgressChangedEventHandler ProgressChanged {
            add { launcher.ProgressChanged += value; } 
            remove { launcher.ProgressChanged -= value; }
        }
        public event DownloadFileChangedHandler FileChanged
        {
            add { launcher.FileChanged += value; }
            remove { launcher.FileChanged -= value; }
        }

        private string userName;
        public string? UserName {
            get { return userName; }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    userName = value;
                    this.launchOption.Session = MSession.GetOfflineSession(value);
                }
                else
                {
                    userName = "no_name";
                    this.launchOption.Session = MSession.GetOfflineSession("no_name");
                }
            }
        }

        public MGame(string? gamePath)
        {
            if (gamePath != null && gamePath.Substring(1, 2).Equals(":/"))
            {
                this.GamePath = new MinecraftPath(gamePath);
            }

            this.launcher = new CMLauncher(GamePath);

            System.Net.ServicePointManager.DefaultConnectionLimit = 256;
            this.launcher.FileDownloader = new AsyncParallelDownloader();

            this.launchOption = new MLaunchOption
            {
                FullScreen = false
            };

            this.UserName = null;

            OutputRecieved += (msg, uis) => { return; };
            ProcessExited += (msg, uis) => { return; };
        }

        public MGame() : this(null)
        {
        }
        
        public async Task StartGame()
        {
            if (!string.IsNullOrWhiteSpace(GameVersion))
            {
                var process = await launcher.CreateProcessAsync(GameVersion, launchOption);

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.EnableRaisingEvents = true;
                process.ErrorDataReceived += OutputRecieved;
                process.OutputDataReceived += OutputRecieved;
                process.Exited += ProcessExited;

                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
            else
            {
                throw new VersionIsNullException($"Invalid version name: {GameVersion}");
            }
        }
    }

    class VersionIsNullException : Exception
    {
        public VersionIsNullException(string message)
        : base(message) { }
    }
}
