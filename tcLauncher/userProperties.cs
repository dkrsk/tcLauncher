using System.Text.Json;
using System.Diagnostics;

namespace DnKR.tcLauncher
{
    
    internal class UserProperties
    {
        public string? nickname { get; set; }
        public string? javaArgs { get; set; }
        public string? javaPath { get; set; }
        public string? ram { get; set; }
        public string? latestVersion { get; set; }
        public bool autoUpdate { get; set; } = false;
        public bool devOps { get; set; } = false;

        private string path;
        private readonly string fileName = "\\tclauncher_properties.json";

        public UserProperties()
        {
            this.path = MainForm.gamePath.ToString();
        }

        public async void JsonWrite()
        {
            using (FileStream fs = new FileStream(path + fileName, FileMode.Create))
            {
                Debug.WriteLine(this.nickname);
                await JsonSerializer.SerializeAsync(fs, this);
            }
        }

        public async Task JsonRead()
        {
            Debug.WriteLine(File.Exists(path + fileName));
            Debug.WriteLine(path + fileName);
            if (!File.Exists(path + fileName))
                return;
            using (FileStream fs = new FileStream(path + fileName, FileMode.OpenOrCreate))
            {
                UserProperties? readed = await JsonSerializer.DeserializeAsync<UserProperties>(fs);
                if (readed != null)
                {
                    this.nickname = readed.nickname;
                    this.javaArgs = readed.javaArgs;
                    this.javaPath = readed.javaPath;
                    this.ram = readed.ram;
                    this.latestVersion = readed.latestVersion;
                    this.autoUpdate = readed.autoUpdate;
                    this.devOps = readed.autoUpdate;
                }
            }
        }
    }
}
