using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.Installer.QuiltMC;

using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DnKR.tcLauncher
{
    public partial class InstallQuiltForm : Form
    {
        CMLauncher launcher;
        QuiltVersionLoader quiltLoader = new QuiltVersionLoader();
        MVersionCollection versions;

        public InstallQuiltForm(CMLauncher launcher)
        {
            this.launcher = launcher;

            InitializeComponent();
        }


        private async void InstallFabricForm_Shown(object sender, EventArgs e)
        {
            launcher.ProgressChanged += Launcher_ProgressChanged;
            cbVersion.Items.Clear();

            //quiltLoader.LoaderVersion = "0.13.3";
            //0.16.0-beta.8
            this.versions = await quiltLoader.GetVersionMetadatasAsync();
            Regex regex = new Regex(@"\d\.\d\d\.\d$");

            foreach (var item in versions)
            {
                if (regex.IsMatch(item.Name))
                    cbVersion.Items.Add(item.Name);
            }
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;

            var quilt = versions.GetVersionMetadata(cbVersion.Text);
            await quilt.SaveAsync(launcher.MinecraftPath);


            MessageBox.Show("Success!");
            this.Close();
        }


        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pb_Progress.Maximum = 100;
            pb_Progress.Value = e.ProgressPercentage;
        }
    }
}
