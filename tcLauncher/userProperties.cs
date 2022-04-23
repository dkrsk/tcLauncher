using System.Text.Json;
using System.Diagnostics;

namespace DnKR.tcLauncher
{
    
    internal class UserProperties
    {
        public string? nickname { get; set; }
        public string? javaArgs { get; set; } = "-XX:+UseG1GC -XX:+ParallelRefProcEnabled -XX:MaxGCPauseMillis=200 -XX:+UnlockExperimentalVMOptions -XX:+DisableExplicitGC -XX:+AlwaysPreTouch -XX:G1NewSizePercent=30 -XX:G1MaxNewSizePercent=40 -XX:G1HeapRegionSize=8M -XX:G1ReservePercent=20 -XX:G1HeapWastePercent=5 -XX:G1MixedGCCountTarget=4 -XX:InitiatingHeapOccupancyPercent=15 -XX:G1MixedGCLiveThresholdPercent=90 -XX:G1RSetUpdatingPauseTimePercent=5 -XX:SurvivorRatio=32 -XX:+PerfDisableSharedMem -XX:MaxTenuringThreshold=1";
        public string? javaPath { get; set; }
        public string? ram { get; set; } = "2048";
        public string? latestVersion { get; set; }
        public string? bkgPath { get; set; } = null;
        public bool autoUpdate { get; set; } = false;

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
                    this.bkgPath = readed.bkgPath;
                    this.autoUpdate = readed.autoUpdate;
                }
            }
        }
    }
}
