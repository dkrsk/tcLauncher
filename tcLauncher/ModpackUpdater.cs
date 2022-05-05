using System.Net;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;


namespace DnKR.tcLauncher.tcUpdater
{
    public class UpdateBuilder
    {
        private readonly string path;
        private readonly string uri;
        private readonly string ftpPath;
        private readonly NetworkCredential credentials;

        public string[] RemoveFilesList { get; set; } = { "mods", "moddata", "config", ".fabric", "resources", "shaderpacks" };

        public UpdateBuilder(string path, string uri, string ftpPath, NetworkCredential credentials){
            this.path = Path.GetFullPath(path);
            this.uri = uri;
            this.ftpPath = ftpPath;
            this.credentials = credentials;
        }

        public UpdateBuilder(UpdaterConfig config)
        {
            this.path = Path.GetFullPath(config.path);
            this.uri = config.uri;
            this.ftpPath = config.ftpPath;
            this.credentials = config.credential;
        }

        private FtpWebRequest CreateRequest(string method)
        {
            return CreateRequest("",method);
        }

        public FtpWebRequest CreateRequest(string FileName, string method)
        {
            FtpWebRequest rq = (FtpWebRequest)WebRequest.Create(Path.Combine(uri,ftpPath,FileName));

            rq.UseBinary = true;
            rq.Credentials = this.credentials;
            rq.Method = method;

            return rq;
        }

        public string[] GetNames()
        {

            var list = new List<string>();

            var request = CreateRequest(WebRequestMethods.Ftp.ListDirectory);

            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), true);

            while (!reader.EndOfStream)
            {
                list.Add(reader.ReadLine());
            }

            return list.ToArray();
        }

        public async Task<string[]> GetNamesAsync()
        {
            return await Task.Run(() => GetNames());
        }

        public void DownloadFile(string PackageName)
        {
            string realPath = Path.Combine(path, PackageName);

            var request = CreateRequest(PackageName, WebRequestMethods.Ftp.DownloadFile);

            const int bufferSize = 131072;
            byte[] buffer = new byte[bufferSize];

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            using (var fs = new FileStream(realPath, FileMode.Create))
            {
                long position = 0;
				int readCount = stream.Read(buffer, 0, bufferSize);
 
				while (readCount > 0)
                {
					fs.Write(buffer, 0, readCount);
					readCount = stream.Read(buffer, 0, bufferSize);
                    position += readCount;
                }
			}

        }

        public async Task DownloadFileAsync(string PackageName)
        {
            await Task.Run(() => DownloadFile(PackageName));
        }

        public void ExtractFile(string PackageName)
        {
            string realPath = Path.Combine(path, PackageName);

            foreach(string dir in RemoveFilesList)
            {
                if (Directory.Exists(Path.Combine(path, dir)))
                {
                    Directory.Delete(Path.Combine(path, dir), true);
                }
            }

            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(realPath)))
            {
                ZipEntry entry;

                while ((entry = zipStream.GetNextEntry()) != null)
                {
                    if (entry.IsFile)
                    {
                        FileStream streamWriter = new FileStream(Path.Combine(path, entry.Name), FileMode.Create);

                        const int bufferSize = 8192;
                        byte[] buffer = new byte[bufferSize];

                        int readCount = zipStream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            streamWriter.Write(buffer, 0, readCount);
                            readCount = zipStream.Read(buffer, 0, bufferSize);
                        }
                        streamWriter.Close();
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.Combine(path, entry.Name));
                    }
                }
            }

            File.Delete(realPath);
        }

        public async Task ExtractFileAsync(string PackageName)
        {
            await Task.Run(() => ExtractFile(PackageName));
        }

    }

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