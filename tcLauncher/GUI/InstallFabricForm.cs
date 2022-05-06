using System.Text.RegularExpressions;

using DnKR.tcLauncher.MVersionInstaller;

namespace DnKR.tcLauncher.GUI
{
    public partial class InstallFabricForm : Form
    {
        readonly FabricInstaller fabricInstaller;

        public InstallFabricForm(MGame game)
        {
            this.fabricInstaller = new(game);
            InitializeComponent();
        }


        private async void InstallFabricForm_Shown(object sender, EventArgs e)
        {
            cbVersion.Items.Clear();

            var versions = await fabricInstaller.GetFabricMetadatas();
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
                await fabricInstaller.InstallFabric(cbVersion.Text);

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
