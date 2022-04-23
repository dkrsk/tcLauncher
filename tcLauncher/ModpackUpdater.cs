using System.Diagnostics;
using System.Net;
using Ionic.Zip;


namespace DnKR.tcLauncher.tcUpdater
{
    public class modpackUpdater
    {
        private string path;
        private string uri;
        private string ftpPath;
        private NetworkCredential credentials;

        public string[] RemoveFilesList { get; set; } = { "mods", "moddata", "config", ".fabric", "resources", "shaderpacks" };

        public modpackUpdater(string path, string uri, string ftpPath, NetworkCredential credentials){
            this.path = path + "\\";
            this.uri = uri;
            this.ftpPath = ftpPath;
            this.credentials = credentials;
        }

        public modpackUpdater(UpdaterConfig config)
        {
            this.path = config.path + "\\";
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
            FtpWebRequest rq = (FtpWebRequest)WebRequest.Create(this.uri + ftpPath + FileName);

            rq.Credentials = this.credentials;
            rq.Method = method;

            return rq;
        }

        public string[] GetNames()
        {

            var list = new List<string>();

            var request = createRequest(WebRequestMethods.Ftp.ListDirectory);

            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), true);

            while (!reader.EndOfStream) {
                list.Add(reader.ReadLine());
            }

            return list.ToArray();
        }

        public void DownloadFile(string PackageName)
        {
            int bufferSize = 131072;

            var request = createRequest(PackageName, WebRequestMethods.Ftp.DownloadFile);

            byte[] buffer = new byte[bufferSize];
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            using (var fs = new FileStream(path + PackageName, FileMode.OpenOrCreate)) {
				int readCount = stream.Read(buffer, 0, bufferSize);
 
				while (readCount > 0) {
					fs.Write(buffer, 0, readCount);
					readCount = stream.Read(buffer, 0, bufferSize);
				}
			}

        }

        public void ExtractFile(string PackageName)
        {

            using (ZipFile zipObj = ZipFile.Read(path + PackageName))
            {

                foreach (ZipEntry entry in zipObj)
                {
                    foreach (string dir in RemoveFilesList)
                    {
                        if (Directory.Exists(path + dir) && entry.FileName.Contains(dir))
                        {
                            Directory.Delete(path + dir, true);
                        }
                    }
                }

                zipObj.ExtractAll(path, ExtractExistingFileAction.DoNotOverwrite);
                
            }

            File.Delete(path + PackageName);
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