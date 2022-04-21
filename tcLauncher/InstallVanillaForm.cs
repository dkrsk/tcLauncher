using CmlLib.Core;
using CmlLib.Core.Downloader;

using System.ComponentModel;


namespace DnKR.tcLauncher
{
    public partial class InstallVanillaForm : Form
    {
        CMLauncher launcher;
        public InstallVanillaForm(CMLauncher launcher)
        {
            this.launcher = launcher;
            InitializeComponent();
        }


        private async void InstallVanillaForm_Shown(object sender, EventArgs e)
        {
            launcher.ProgressChanged += Launcher_ProgressChanged;
            cbVersion.Items.Clear();

            var versions = await launcher.GetAllVersionsAsync();


            foreach (var item in versions)
            {
                cbVersion.Items.Add(item.Name);
            }
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;
            launcher.FileDownloader = new AsyncParallelDownloader();

            await launcher.CheckAndDownloadAsync(await launcher.GetVersionAsync(cbVersion.Text));

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
