using CmlLib.Core.Version;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.Installer.QuiltMC;

namespace DnKR.tcLauncher.MVersionInstaller
{
    public class FabricInstaller
    {
        private readonly MGame game;
        private readonly FabricVersionLoader fabricLoader;
        private string loaderVersion = "0.13.3";
        
        public string LoaderVersion {
            get { return loaderVersion; }
            set { loaderVersion = value; } }

        public FabricInstaller(MGame game)
        {
            this.game = game;

            this.fabricLoader = new FabricVersionLoader();
            this.fabricLoader.LoaderVersion = loaderVersion;
        }

        public async Task<MVersionCollection> GetFabricMetadatas()
        {
            return await fabricLoader.GetVersionMetadatasAsync();
        }

        public async Task InstallFabric(string version)
        {
            MVersionCollection versions = await fabricLoader.GetVersionMetadatasAsync();

            var fabric = versions.GetVersionMetadata(version);
            await fabric.SaveAsync(game.launcher.MinecraftPath);

            await game.launcher.CheckAndDownloadAsync(await fabric.GetVersionAsync());
        }
    }

    public class QuiltInstaller
    {
        readonly MGame game;
        readonly QuiltVersionLoader quiltLoader;

        public QuiltInstaller(MGame game)
        {
            this.game = game;

            this.quiltLoader = new QuiltVersionLoader();
        }

        public async Task<MVersionCollection> GetQuiltMetadatas()
        {
            return await quiltLoader.GetVersionMetadatasAsync();
        }

        public async Task InstallQuilt(string version)
        {
            MVersionCollection versions = await quiltLoader.GetVersionMetadatasAsync();

            var quilt = versions.GetVersionMetadata(version);
            await quilt.SaveAsync(game.launcher.MinecraftPath);

            await game.launcher.CheckAndDownloadAsync(await quilt.GetVersionAsync());
        }
    }

    public class VanillaInstaller
    {
        MGame game;

        public VanillaInstaller(MGame game)
        {
            this.game = game;
        }

        public async Task<MVersionCollection> GetVanillaVersions()
        {
            return await game.launcher.GetAllVersionsAsync();
        }

        public async Task InstallVanilla(string version)
        {
            await game.launcher.CheckAndDownloadAsync(await game.launcher.GetVersionAsync(version));
        }
    }

}
