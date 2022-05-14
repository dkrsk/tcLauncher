using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;
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

        public UpdateBuilder(string path, string uri, string ftpPath, NetworkCredential credentials)
        {
            this.path = Path.GetFullPath(path);
            this.uri = uri;
            this.ftpPath = ftpPath;
            this.credentials = credentials;
        }

        public UpdateBuilder(UpdaterConfig config) : this(config.path, config.uri, config.ftpPath, config.credential)
        {

        }

        private FtpWebRequest CreateRequest(string method)
        {
            return CreateRequest("", method);
        }

        private FtpWebRequest CreateRequest(string FileName, string method)
        {
            FtpWebRequest rq = (FtpWebRequest)WebRequest.Create(Path.Combine(uri, ftpPath, FileName));

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
            return await Task.Run(() => GetNames()).ConfigureAwait(false);
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
            await Task.Run(() => DownloadFile(PackageName)).ConfigureAwait(false);
        }

        public void ExtractFile(string PackageName)
        {
            string realPath = Path.Combine(path, PackageName);

            foreach (string dir in RemoveFilesList)
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
            await Task.Run(() => ExtractFile(PackageName)).ConfigureAwait(false);
        }

    }
}
