using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.Installer.FabricMC;

using System.ComponentModel;

namespace DnKR.tcLauncher
{
    public partial class InstallFabricForm : Form
    {
        CMLauncher launcher;
        FabricVersionLoader fabricLoader = new FabricVersionLoader();
        MVersionCollection versions;

        public InstallFabricForm(CMLauncher launcher)
        {
            this.launcher = launcher;

            InitializeComponent();
        }


        private async void InstallFabricForm_Shown(object sender, EventArgs e)
        {
            launcher.ProgressChanged += Launcher_ProgressChanged;
            cbVersion.Items.Clear();

            this.versions = await fabricLoader.GetVersionMetadatasAsync();


            foreach (var item in versions)
            {
                cbVersion.Items.Add(item.Name);
            }
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;

            var fabric = versions.GetVersionMetadata(cbVersion.Text);
            await fabric.SaveAsync(launcher.MinecraftPath);
            

            MessageBox.Show("Succes!");
            this.Close();
        }


        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pb_Progress.Maximum = 100;
            pb_Progress.Value = e.ProgressPercentage;
        }
    }
}
