using System.Text.Json;

namespace DnKR.tcLauncher
{
    
    internal class UserProperties
    {
        public string? Nickname { get; set; }
        public string? JavaArgs { get; set; } = "-XX:+UseG1GC -XX:+ParallelRefProcEnabled -XX:MaxGCPauseMillis=200 -XX:+UnlockExperimentalVMOptions -XX:+DisableExplicitGC -XX:+AlwaysPreTouch -XX:G1NewSizePercent=30 -XX:G1MaxNewSizePercent=40 -XX:G1HeapRegionSize=8M -XX:G1ReservePercent=20 -XX:G1HeapWastePercent=5 -XX:G1MixedGCCountTarget=4 -XX:InitiatingHeapOccupancyPercent=15 -XX:G1MixedGCLiveThresholdPercent=90 -XX:G1RSetUpdatingPauseTimePercent=5 -XX:SurvivorRatio=32 -XX:+PerfDisableSharedMem -XX:MaxTenuringThreshold=1";
        public string? JavaPath { get; set; }
        public string? Ram { get; set; } = "2048";
        public string? LatestVersion { get; set; }
        public string? BkgPath { get; set; }

        private readonly string path;
        private readonly string fileName = "\\tclauncher_properties.json";

        public UserProperties()
        {
            this.path = MainForm.gamePath.ToString();
        }

        public async Task JsonWrite()
        {
            using (FileStream fs = new FileStream(path + fileName, FileMode.Create))
            {
                await JsonSerializer.SerializeAsync(fs, this);
            }
        }

        public async Task JsonRead()
        {
            if (!File.Exists(path + fileName))
            {
                return;
            }
            using (FileStream fs = new FileStream(path + fileName, FileMode.OpenOrCreate))
            {
                UserProperties? readed = await JsonSerializer.DeserializeAsync<UserProperties>(fs);
                if (readed != null)
                {
                    this.Nickname = readed.Nickname;
                    this.JavaArgs = readed.JavaArgs;
                    this.JavaPath = readed.JavaPath;
                    this.Ram = readed.Ram;
                    this.LatestVersion = readed.LatestVersion;
                    this.BkgPath = readed.BkgPath;
                }
            }
        }
    }
}
