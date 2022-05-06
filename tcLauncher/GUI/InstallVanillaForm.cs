using DnKR.tcLauncher.MVersionInstaller;


namespace DnKR.tcLauncher.GUI
{
    public partial class InstallVanillaForm : Form
    {
        readonly VanillaInstaller vanillaInstaller;

        public InstallVanillaForm(MGame game)
        {
            this.vanillaInstaller = new(game);
            InitializeComponent();
        }


        private async void InstallVanillaForm_Shown(object sender, EventArgs e)
        {
            cbVersion.Items.Clear();

            var versions = await vanillaInstaller.GetVanillaVersions();


            foreach (var item in versions)
            {
                cbVersion.Items.Add(item.Name);
            }
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;

            if (!string.IsNullOrWhiteSpace(cbVersion.Text))
            {
                await vanillaInstaller.InstallVanilla(cbVersion.Text);

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
