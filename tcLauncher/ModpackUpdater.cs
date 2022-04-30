using System.Net;
using ICSharpCode.SharpZipLib.Zip;


namespace DnKR.tcLauncher.tcUpdater
{
    public class modpackUpdater
    {
        private string path;
        private string uri;
        private string ftpPath;
        private NetworkCredential credentials;

        public string[] RemoveFilesList { get; set; } = { "mods", "moddata", "config", ".fabric", "resources", "shaderpacks" };

        public delegate void ProgressHandler(long length, long position);
        public event ProgressHandler? ProgressChanged;

        public modpackUpdater(string path, string uri, string ftpPath, NetworkCredential credentials){
            this.path = Path.GetFullPath(path);
            this.uri = uri;
            this.ftpPath = ftpPath;
            this.credentials = credentials;
        }

        public modpackUpdater(UpdaterConfig config)
        {
            this.path = Path.GetFullPath(config.path);
            this.uri = config.uri;
            this.ftpPath = config.ftpPath;
            this.credentials = config.credential;
        }

        private FtpWebRequest createRequest(string method)
        {
            return createRequest("",method);
        }

        public FtpWebRequest createRequest(string FileName, string method)
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

            var request = createRequest(WebRequestMethods.Ftp.ListDirectory);

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

            var request = createRequest(PackageName, WebRequestMethods.Ftp.DownloadFile);

            const int bufferSize = 131072;
            byte[] buffer = new byte[bufferSize];

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            //long fileSize = response.ContentLength;

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
                    
                    //if (ProgressChanged != null) ProgressChanged(fileSize, position);
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
                    Directory.Delete(Path.Combine(path, dir), true);
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
                    if (ProgressChanged != null && zipStream.Available == 1 && zipStream.Length != 0) // not stable
                        ProgressChanged(zipStream.Length, zipStream.Position);
                }
            }

            File.Delete(realPath);
        }

        public async Task ExtractFileAsync(string PackageName)
        {
            await Task.Run(() => ExtractFile(PackageName));
        }

        public void UpdateProgress(int length, int position)
        {

        }

    }

    public class UpdaterConfig
    {
        public string path;
        public string uri;
        public string ftpPath;
        public NetworkCredential credential;

        public UpdaterConfig(string path, string uri, string ftpPath, NetworkCredential credential)
        {
            this.path = path;
            this.uri = uri;
            this.ftpPath = ftpPath;
            this.credential = credential;
        }
    }
}