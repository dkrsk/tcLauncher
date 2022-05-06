using System.ComponentModel;
using System.Text.RegularExpressions;

using DnKR.tcLauncher.MVersionInstaller;

namespace DnKR.tcLauncher.GUI
{
    public partial class InstallQuiltForm : Form
    {
        readonly QuiltInstaller quiltInstaller;

        public InstallQuiltForm(MGame game)
        {
            this.quiltInstaller = new(game);

            InitializeComponent();
        }


        private async void InstallFabricForm_Shown(object sender, EventArgs e)
        {
            cbVersion.Items.Clear();

            //quiltLoader.LoaderVersion = "0.13.3";
            //0.16.0-beta.8
            var versions = await quiltInstaller.GetQuiltMetadatas();
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

            if (!string.IsNullOrWhiteSpace(cbVersion.Text))
            {
                await quiltInstaller.InstallQuilt(cbVersion.Text);

                MessageBox.Show("Success!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Select the version first!");
            }

            btnInstall.Enabled = true;
        }
    }
}
